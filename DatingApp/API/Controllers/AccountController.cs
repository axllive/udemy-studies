using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPiController
    {
        public readonly DBContext _context;
        public readonly ITokenService _tokenService;
        public AccountController(DBContext dBContext, ITokenService tokenService)
        {
            _context = dBContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")] //POST: api/account/register
        public async Task<ActionResult<UserDTO>> Register (RegisterDTO registerDTO)
        {
            if ( await UserExists(registerDTO.username))
            {
                return BadRequest("User already exists.");
            }

            using var hmac = new HMACSHA512();

            AppUser usr = new AppUser{
                UserName = registerDTO.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                PasswordSalt = hmac.Key,
                Bio = registerDTO.bio
            };

            _context.Users.Add(usr);
            await _context.SaveChangesAsync();
            return new UserDTO
            {
                username = usr.UserName,
                token = _tokenService.CreateToken(usr)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginData)
        {
            AppUser usr = await _context.Users
            .Include(d => d.Photos)
            .SingleOrDefaultAsync(d => d.UserName == loginData.username);

            if (usr == null) return Unauthorized("User not found.");

            using var hmac = new HMACSHA512(usr.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginData.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usr.PasswordHash[i]) return Unauthorized("Invalid password.");
            }

            return new UserDTO
            {
                username = usr.UserName,
                token = _tokenService.CreateToken(usr),
                currentphotourl = usr.Photos.FirstOrDefault(d => d.IsMain).Url
            };
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }
    }
}