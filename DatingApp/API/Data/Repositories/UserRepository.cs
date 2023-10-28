
using API.Entities;
using API.Interfaces;
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
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(usr => usr.Photos)
            .AsNoTracking()
            .ToListAsync();
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
    }
}