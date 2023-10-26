using System.Security.Claims;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegisterDTO>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        
        if (users.Any()) return users.Adapt<List<RegisterDTO>>();
        else return NotFound("Database returned no users");
            
    }

    [AllowAnonymous]
    [HttpGet("user/{id}/")]
    public async Task<ActionResult<RegisterDTO>> GetUser(int id)
    {
       AppUser usr = await _userRepository.GetUserByIdAsync(id);
       return usr.Adapt<RegisterDTO>();
    }

    [AllowAnonymous]
    [HttpGet("user/byname/{name}/")]
    public async Task<ActionResult<RegisterDTO>> GetUserByName(string name)
    {
        AppUser usr = await _userRepository.GetUserByUsernameAsync(name);
        return usr.Adapt<RegisterDTO>();

    }

    public static string GetHash(string input)
    {
        int index = input.IndexOf('.');
        if (index == -1) return "";

        index = input.IndexOf('.', index + 1);
        if (index == -1) return "";

        return input.Substring(index + 1);
    }       

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

        string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        AppUser user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) return NotFound();

        memberUpdateDto.Adapt(user);

        _userRepository.Update(user);

        if (await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Falied to update user");

    }
}
