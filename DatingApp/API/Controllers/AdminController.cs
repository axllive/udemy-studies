using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController: BaseAPiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            
        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var usrs = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new 
                {
                    u.Id,
                    username = u.UserName,
                    roles = u.UserRoles.Select( r=> r.Role.Name).ToList()
                })
                .ToListAsync();

                return Ok(usrs);
        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if(string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role.");

            var selectedRoles = roles.Split(",").ToArray();

            var usr = await _userManager.FindByNameAsync(username);

            if(usr == null) return NotFound();

            var usrRoles = await _userManager.GetRolesAsync(usr);
            var result = await _userManager.AddToRolesAsync(usr, selectedRoles.Except(usrRoles));

            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(usr, usrRoles.Except(selectedRoles));

            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(usr));
        }

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModerations()
        {
            return Ok("Admins or moderator can see this");
        }
    }
}