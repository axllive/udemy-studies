using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UserController : ControllerBase
{
    public DBContext _context { get; }
    public UserController(DBContext dBContext)
    {
        _context = dBContext;
    }

}
