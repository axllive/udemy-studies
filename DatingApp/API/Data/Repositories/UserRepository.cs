
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;
        public UserRepository(DBContext context)
        {
            _context = context;
        }
        public async Task<PagedList<RegisterDTO>> GetUsersAsync(UserParams usrParams)
        {
            var query = _context.Users
            .Include(usr => usr.Photos)
            .ProjectToType<RegisterDTO>()
            .AsNoTracking();

            return await PagedList<RegisterDTO>.CreateAsync(query, usrParams.PageNumber, usrParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(usr => usr.Photos)
            .AsNoTracking()
            .SingleOrDefaultAsync( usr =>usr.UserName.Contains(username));
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser usr)
        {
             _context.Entry(usr).State = EntityState.Modified;
        }

        public async Task<bool> AddPhoto(AppUser user, Photo photo)
        {   
            AppUser usr = await _context.Users.FindAsync(user.Id);
            if (usr != null)
            {
                usr.Photos.Add(photo);                
                return await SaveAllAsync();
            }
            else return false;
        }
        public async Task<bool> SetMainPhoto(AppUser user, Photo photo)
        {   
            AppUser usr = await _context.Users
                .Include( x => x.Photos)
                .Where(x => x.Id == user.Id)
                .FirstOrDefaultAsync();

            if (usr != null)
            {
                Photo currentMain = usr.Photos.FirstOrDefault(x => x.IsMain);

                if(currentMain != null) currentMain.IsMain = false;

                usr.Photos.FirstOrDefault(x => x.Id == photo.Id).IsMain = true;
                
                return await SaveAllAsync();
            }
            else return false;
        }
    }
}