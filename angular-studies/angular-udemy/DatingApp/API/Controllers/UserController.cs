using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")] // /api/users
public class UserController : BaseAPiController
{
    private readonly DBContext _context;

    public UserController(DBContext dBContext)
    {
        _context = dBContext;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(string jsonUsr)
    {
        if (jsonUsr == null)
        {
            return Unauthorized();
        }

        LoginDTO loginData = JsonSerializer.Deserialize<LoginDTO>(jsonUsr);
        AppUser usr = await _context.Users.SingleOrDefaultAsync(d => d.UserName == loginData.username);

        if (usr == null) return Unauthorized("User not found.");

        var users = await _context.Users.ToListAsync();
        
        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
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
