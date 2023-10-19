using System.Text.Json;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")] // /api/users
public class UserController : BaseAPiController
{
    public readonly ITokenService _tokenService;
    public readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegisterDTO>>> GetUsers(string jsonUsr)
    {
        if (jsonUsr == null) return Unauthorized();

        try
        {  
            LoginDTO loginData = JsonSerializer.Deserialize<LoginDTO>(jsonUsr);
            AppUser usr = await _userRepository.GetUserByUsernameAsync(loginData.username);

            if (usr == null) return Unauthorized("User not found.");

            if ( ! _tokenService.ValidateToken(loginData.token) ) return Unauthorized("Token coulndnt be validated");

            var users = await _userRepository.GetUsersAsync();
            
            if (users.Any()) return users.Adapt<List<RegisterDTO>>();
            else return NotFound("Database returned no users");
            
        }
        catch (System.Exception e)
        {
            
            return Unauthorized(e.ToString());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<AppUser>> GetUserByName(string name)
    {
        return await _userRepository.GetUserByUsernameAsync(name);
    }

    public static string GetHash(string input)
    {
        int index = input.IndexOf('.');
        if (index == -1) return "";

        index = input.IndexOf('.', index + 1);
        if (index == -1) return "";

        return input.Substring(index + 1);
    }       

}
