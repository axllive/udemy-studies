using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPiController
    {
        public readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")] //POST: api/account/register
        public async Task<ActionResult<UserDTO>> Register (RegisterDTO registerDTO)
        {
            if ( await UserExists(registerDTO.username))
            {
                return BadRequest("User already exists.");
            }

            AppUser usr = registerDTO.Adapt<AppUser>();

            
            usr.UserName = registerDTO.username.ToLower();
            usr.Photos.Add(
                new Photo(){
                    IsMain = true,
                    Url = $"https://upload.wikimedia.org/wikipedia/commons/2/2f/No-photo-m.png"
                }
            );

            var result = await _userManager.CreateAsync(usr, registerDTO.password);

            if(!result.Succeeded) return BadRequest(result.Errors);
            
            var roleResult = await _userManager.AddToRoleAsync(usr, "Member");
            
            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var roles = await _userManager.GetRolesAsync(usr);

            return new UserDTO
            {
                username = usr.UserName,
                gender = usr.Gender,
                token = await _tokenService.CreateToken(usr),
                knownas = usr.KnownAs,
                currentphotourl =$"https://upload.wikimedia.org/wikipedia/commons/2/2f/No-photo-m.png",
                roles = roles.ToArray()
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginData)
        {
            AppUser usr = await _userManager.Users
            .Include(d => d.Photos)
            .SingleOrDefaultAsync(d => d.UserName == loginData.username);

            if (usr == null) return Unauthorized("User not found.");

            var result = await _userManager.CheckPasswordAsync(usr, loginData.password);

            if(!result) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(usr);

            UserDTO usrn = new UserDTO
            {
                username = usr.UserName,
                token = await _tokenService.CreateToken(usr),
                currentphotourl = usr.Photos.FirstOrDefault(d => d.IsMain) == null ? "" : usr.Photos.FirstOrDefault(d => d.IsMain).Url,
                gender = usr.Gender,
                knownas = usr.KnownAs,
                roles = roles.ToArray()
            };
            return usrn;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }
    }
}