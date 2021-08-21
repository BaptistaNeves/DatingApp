using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<MemberDto> GetMemberByUsernameAsync(string username)
        {
            // return await _context.Users
            //             .Where(x => x.UserName == username)
            //             .Select(user => new MemberDto 
            //             {
            //                 Id = user.Id,
            //                 Username = user.UserName
            //             }).SingleOrDefaultAsync();

            return await _context.Users.Where(x => x.UserName == username)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            /*Adding AsQueryable() give us the opportunity to do something with this
            query and decidy what we want to filter by, for instance.*/
            //ProjectTo<> Give us the hability to select what property we want to brign in 
            var query =  _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUserName);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper
                        .ConfigurationProvider)
                        .AsNoTracking(), 
                        userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetUserIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                        .Include(p => p.Photos)
                        .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser appUser)
        {
            _context.Entry(appUser).State = EntityState.Modified;
        }
    }
}