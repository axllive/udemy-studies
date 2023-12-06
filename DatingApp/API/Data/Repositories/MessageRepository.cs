using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Mapster;
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

        public async Task<PagedList<ChatedMemberDTO>> GetChatedUsers(string username)
        {
            IEnumerable<Message> msgs = await _context.Messages
                .Include( x => x.Sender)
                    .ThenInclude(x => x.Photos)
                .Include( x => x.Recipient)
                    .ThenInclude(x => x.Photos)
                .Where(x => x.SenderUsername == username || x.RecipientUsername == username)
                .AsNoTracking()
                .ToListAsync();
            
            List<ChatedMemberDTO> chatedUsrs = new();
            
            foreach (var item in msgs)
            {
                if(item.SenderUsername == username){
                    chatedUsrs.Add(
                        new(){
                            username = item.Recipient.UserName,
                            knownas = item.Recipient.KnownAs,
                            lastactive = item.Recipient.LastActive,
                            photourl = item.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url
                        }
                    );
                }
                else if(item.RecipientUsername == username){
                    chatedUsrs.Add(
                        new(){
                            username = item.Sender.UserName,
                            knownas = item.Sender.KnownAs,
                            lastactive = item.Sender.LastActive,
                            photourl = item.Sender.Photos.FirstOrDefault(x => x.IsMain).Url
                        }
                    );
                }
            }
            
            MessageParams messageParams= new(){
                Username = username
            };
            return await PagedList<ChatedMemberDTO>
                .CreateFromListAsync(chatedUsrs.DistinctBy(x => x.username).ToList(), messageParams.PageNumber, messageParams.PageSize);
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
    }
}