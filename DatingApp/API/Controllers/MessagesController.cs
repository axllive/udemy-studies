using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController: BaseAPiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            string usrname = User.GetUsername();

            if(usrname == createMessageDTO.recipientusername.ToLower()) 
                return BadRequest("You cant message yourself goddamnt!!!");

            AppUser sender = await _userRepository.GetUserByUsernameAsync(usrname);
            AppUser recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.recipientusername);

            if(recipient == null) return NotFound();

            Message msg = new(){
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.content
            };
            
            _messageRepository.AddMessage(msg);

            msg.Sender = sender;
            msg.Recipient = recipient;

            if(await _messageRepository.SaveAllAsync()) return Created("/", msg.Adapt<MessageDTO>());

            return BadRequest("Failed to send message.;");
        }
    
        [HttpGet("chatUsers")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetChatedUsers(){
            string usrname = User.GetUsername();

            IEnumerable<AppUser> chatedUsrs = await _messageRepository.GetChatedUsers(usrname);

            chatedUsrs = chatedUsrs.DistinctBy(x => x.UserName);

            return chatedUsrs.Adapt<List<MemberDTO>>();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser( [FromQuery] MessageParams messageParams )
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(
                new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages)
            );

            return messages;
        }
    
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUserName = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUserName, username));
        }
    }
} 