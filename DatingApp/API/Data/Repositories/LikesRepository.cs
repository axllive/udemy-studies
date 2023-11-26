using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Mapster;
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

        public async Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.predicate == "liked"){
                likes =likes.Where(like => like.SoucerUserId == likesParams.userId);
                users = likes.Select(like => like.TargetUser);
            }
            if(likesParams.predicate == "likedBy"){
                likes =likes.Where(like => like.TargetUserId == likesParams.userId);
                users = likes.Select(like => like.SoucerUser);
            }

            var likedUsers = users.Select(usr => new LikeDTO{
                username = usr.UserName,
                kwonas = usr.KnownAs,
                age = usr.GetAge(),
                photourl = usr.Photos.FirstOrDefault(x => x.IsMain).Url,
                city = usr.City,
                id = usr.Id,
                photos = usr.Photos.Adapt<List<PhotoDTO>>()
            });

            return await PagedList<LikeDTO>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync( x => x.Id == userId);
        }
    }
}