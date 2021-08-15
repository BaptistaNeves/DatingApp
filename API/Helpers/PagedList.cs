using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        //'IEnumerable<T> items', this is where we pass in the items that we get from our query. 
        //'count', this counts how many items we get from the query.
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items); // do this will give access to this items inside our pageList.

        }

        public int CurrentPage { get; set; }
        //Which pages we´re on (page 1,2,3, etc)
        public int TotalPages { get; set; }
        //Total pages we have (10 pages, 5 pages, etc)
        public int PageSize { get; set; }
        //How big the page it is (How many items has a page).
        public int TotalCount { get; set; }
        //This counts How items are in this query, This could be all of the items, or based in our query
        //How many total female users ara available based in our query for example.


        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //this works out the total numbers of records that we're goin to return
            //This makes a database call.
            var count = await source.CountAsync(); 
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}