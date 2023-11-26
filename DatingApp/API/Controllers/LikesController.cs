using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
            int sourceId = int.Parse(User.GetUserId());
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
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes(string predicate)
        {
            IEnumerable<LikeDTO> usrs = await _likesRepository.GetUserLikes(predicate, int.Parse(User.GetUserId()));
            return Ok(usrs);
        }
    }
}