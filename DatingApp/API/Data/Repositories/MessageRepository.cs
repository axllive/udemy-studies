using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DBContext _context;

        public MessageRepository(DBContext context)
        {
            _context = context;
        }

        public void AddMessage(Message msg)
        {
            _context.Messages.Add(msg);
        }

        public void DeleteMessage(Message msg)
        {
            _context.Messages.Remove(msg);
        }

        public async Task<Message> FindMessageByID(int messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);
        }

        public async Task<PagedList<ChatedMemberDTO>> GetChatedUsers([FromQuery] MessageParams messageParams)
        {
            var query = from message in _context.Messages
                where message.SenderUsername == messageParams.Username || message.RecipientUsername == messageParams.Username 
                select new ChatedMemberDTO
                {
                    username = message.SenderUsername == messageParams.Username ? message.Recipient.UserName: message.Sender.UserName,
                    knownas = message.SenderUsername == messageParams.Username ? message.Recipient.KnownAs: message.Sender.KnownAs,
                    lastactive = message.SenderUsername == messageParams.Username ? message.Recipient.LastActive: message.Sender.LastActive,
                    photourl = message.SenderUsername == messageParams.Username ? message.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url
                                                                                : message.Sender.Photos.FirstOrDefault(x => x.IsMain).Url
                };

            /* var chattedMembers = await query.DistinctBy(x => x.username)
                                            .AsQueryable(); */
                  
            return await PagedList<ChatedMemberDTO>
                .CreateFromListAsync(query.ToList().DistinctBy(x => x.username), messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending( x => x.MessageSent )
                .AsQueryable();

            query = messageParams.Container switch{
                "Inbox" => query.Where( u => u.RecipientUsername == messageParams.Username ),
                "Outbox" => query.Where( u => u.SenderUsername == messageParams.Username ),
                _ => query.Where( u => u.RecipientUsername == messageParams.Username && u.DateRead == null )
            };

            var messages = query.ProjectToType<MessageDTO>();

            return await PagedList<MessageDTO>
                .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName,string recipientUserName)
        {
            var messages = await _context.Messages
                .Include( x => x.Sender )
                    .ThenInclude( p =>p.Photos )
                .Include( x => x.Recipient )
                    .ThenInclude( p =>p.Photos )
                .Where(
                    m => m.RecipientUsername == currentUserName &&
                    m.SenderUsername == recipientUserName ||
                    m.RecipientUsername == recipientUserName &&
                    m.SenderUsername == currentUserName
                )
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
                
                var unreadMessages = messages
                    .Where( m => m.DateRead == null && m.RecipientUsername == currentUserName  )
                    .ToList();
                
                if(unreadMessages.Any())
                {
                    foreach (var msg in unreadMessages)
                    {
                        msg.DateRead = DateTime.UtcNow;
                    }
                    await _context.SaveChangesAsync();
                }
                    
            return messages.Adapt<IEnumerable<MessageDTO>>();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> Unsend(Message msg)
        {
            _context.Entry(msg).State = EntityState.Deleted;
            return SaveAllAsync();
        }
    }
}