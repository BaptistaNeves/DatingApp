using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, 
                                   IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
            //The RequesteDelete is what is coming up next in the middleware pipeline
            //ILogger - We'll bring ILogger so that can still log out our exception into the terminal
            //IHostEnvironment - To check what environment we're running in
        }

        //InvokeAsync - This is a required method for this middleware
        /*HttpContext - We bring a HttpContext as paremeter,because this is happening
          in the context of HttpContext Resquest, when we add a middleware we have access
          to the actual HttpContext Request that's coming in
        */
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message); // To log out the exception into the terminal
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal server Error");
                
                //To ensure that our response just goes back as a normal json formatted response in a camelcase
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}