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
public class UsersController : BaseAPiController
{
    public readonly ITokenService _tokenService;
    public readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository, ITokenService tokenService)
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

    [AllowAnonymous]
    [HttpGet("user/{id}/{jsonUsr}")]
    public async Task<ActionResult<AppUser>> GetUser(int id, string jsonUsr)
    {
        if (await TokenValidator(jsonUsr)) return await _userRepository.GetUserByIdAsync(id);
        else return Unauthorized("Not authenticated.");
    }

    [AllowAnonymous]
    [HttpGet("user/byname/{name}/{jsonUsr}")]
    public async Task<ActionResult<AppUser>> GetUserByName(string name, string jsonUsr)
    {
        if (await TokenValidator(jsonUsr)) return await _userRepository.GetUserByUsernameAsync(name);
        else return Unauthorized("Not authenticated.");
    }

    public static string GetHash(string input)
    {
        int index = input.IndexOf('.');
        if (index == -1) return "";

        index = input.IndexOf('.', index + 1);
        if (index == -1) return "";

        return input.Substring(index + 1);
    }       

    private async Task<bool> TokenValidator(string jsonObject){
        
        try
        {  
            LoginDTO loginData = JsonSerializer.Deserialize<LoginDTO>(jsonObject);
            AppUser usr = await _userRepository.GetUserByUsernameAsync(loginData.username);

            if (usr == null) return false;

            return _tokenService.ValidateToken(loginData.token);
        }
        catch (System.Exception e)
        {
            
            return false;
        }
    }

}
