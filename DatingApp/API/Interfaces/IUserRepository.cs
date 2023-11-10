using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser usr);
        Task<bool> SaveAllAsync();
        Task<PagedList<RegisterDTO>> GetUsersAsync(UserParams usrParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        public Task<bool> AddPhoto(AppUser user, Photo photo);
        public Task<bool> SetMainPhoto(AppUser user, Photo photo);
    }
}