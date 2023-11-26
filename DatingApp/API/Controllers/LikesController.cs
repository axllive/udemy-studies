using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController: BaseAPiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
            
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            int sourceId = User.GetUserId();
            AppUser likedUsr = await _userRepository.GetUserByUsernameAsync(username);
            AppUser sourceUsr = await _likesRepository.GetUserWithLikes(sourceId);

            if(likedUsr == null) return NotFound();

            if(sourceUsr.UserName == username) return BadRequest("You cant love yourself! Go love sombodyelse.");

            UserLike userLike = await _likesRepository.GetUserLike(sourceUsr.Id, likedUsr.Id);

            if(userLike != null) return BadRequest("You already liked this user");

            userLike = new UserLike{
                SoucerUserId = sourceId,
                TargetUserId = likedUsr.Id
            };

            sourceUsr.LikedUsers.Add(userLike);

            if(await _userRepository.SaveAllAsync()) return Ok(); //refactor

            return BadRequest("Falied to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes( [FromQuery] LikesParams likesParams)
        {
            likesParams.userId = User.GetUserId();
            PagedList<LikeDTO> usrs = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(
                new PaginationHeader(usrs.CurrentPage,
                usrs.PageSize, usrs.TotalCount, usrs.TotalPages)
            );

            return Ok(usrs);
        }
    }
}