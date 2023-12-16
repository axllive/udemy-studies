using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> FindMessageByID(int messageId);
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName,string recipientUserName);
        Task<PagedList<ChatedMemberDTO>> GetChatedUsers(MessageParams messageParams);
        Task<bool> SaveAllAsync();        
        Task<bool> Unsend(Message msg);
    }
}