using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UserController : ControllerBase
{
    private readonly DBContext _dBContext;

    public UserController(DBContext dBContext)
    {
        _dBContext = dBContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _dBContext.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _dBContext.Users.FindAsync(id);
    }

}
