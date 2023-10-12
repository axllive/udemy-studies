using System.Security.Cryptography;
using System.Text;
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
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(LoginDTO loginData)
    {
        AppUser usr = await _context.Users.SingleOrDefaultAsync(d => d.UserName == loginData.username);

        if (usr == null) return Unauthorized("User not found.");

        using var hmac = new HMACSHA512(usr.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginData.password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != usr.PasswordHash[i]) return Unauthorized("Invalid password.");
        }


        var users = await _context.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }

}
