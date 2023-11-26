using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DBContext _context;
        public LikesRepository(DBContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int soucerUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(soucerUserId, likedUserId);
        }

        public async Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(predicate == "liked"){
                likes =likes.Where(like => like.SoucerUserId == userId);
                users = likes.Select(like => like.TargetUser);
            }
            if(predicate == "likedBy"){
                likes =likes.Where(like => like.TargetUserId == userId);
                users = likes.Select(like => like.SoucerUser);
            }

            return await users.Select(usr => new LikeDTO{
                Username = usr.UserName,
                KnownAs = usr.KnownAs,
                Age = usr.GetAge(),
                PhotoURL = usr.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = usr.City,
                Id = usr.Id
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync( x => x.Id == userId);
        }
    }
}