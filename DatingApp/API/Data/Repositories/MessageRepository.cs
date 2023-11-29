using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;

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

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public Task<PagedList<MessageDTO>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDTO>> GetMessageThreade(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}