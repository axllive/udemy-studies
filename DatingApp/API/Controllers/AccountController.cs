using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Mapster;
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

            AppUser usr = registerDTO.Adapt<AppUser>();

            
                usr.UserName = registerDTO.username.ToLower();
                usr.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password));
                usr.PasswordSalt = hmac.Key;

            _context.Users.Add(usr);
            await _context.SaveChangesAsync();
            return new UserDTO
            {
                username = usr.UserName,
                token = _tokenService.CreateToken(usr),
                knownas = usr.KnownAs
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

            UserDTO usrn = new UserDTO
            {
                username = usr.UserName,
                token = _tokenService.CreateToken(usr),
                currentphotourl = usr.Photos.FirstOrDefault(d => d.IsMain) == null ? "" : usr.Photos.FirstOrDefault(d => d.IsMain).Url,
                gender = usr.Gender
            };
            return usrn;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }
    }
}