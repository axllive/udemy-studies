using System.Security.Claims;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
    public readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, ITokenService tokenService, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _photoService = photoService;
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
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        AppUser user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        memberUpdateDto.Adapt(user);

        _userRepository.Update(user);

        if (await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Falied to update user");
    } 

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        AppUser user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        Photo photo = new(){
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0) photo.IsMain = true;

        if(await _userRepository.AddPhoto(user, photo))
        {
            return CreatedAtAction(
                nameof(GetUserByName),  
                new { name = user.UserName }, 
                photo.Adapt<PhotoDTO>()
                );
        }

        return BadRequest("error adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId){

        AppUser usr = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (usr == null) return NotFound();

        Photo photo = usr.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This photo is already your main photo.");

        if (await _userRepository.SetMainPhoto(usr, photo)) return NoContent();
        else return BadRequest("Error setting the main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<IActionResult> DeletePhoto(int photoId){
        AppUser usr = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (usr == null) return NotFound();

        Photo photo = usr.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("Cant delete your main photo.");

        if(photo.PublicId != null){
            var result = await _photoService.DeletePhotoAsync(photo);
            if(result.Error != null) return BadRequest(result.Error.Message);
            else if ( ! await _userRepository.SaveAllAsync() ) return BadRequest(("Error on deleting photo"));
        }
        return Ok();
    }
}
