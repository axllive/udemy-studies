using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MessageDTO
    {
        public int id { get; set; }
        public int senderid { get; set; }
        public string senderusername { get; set; }
        public string senderphotourl { get; set; }
        public int recipientid { get; set; }
        public string recipientusername { get; set; }
        public string recipientphotourl { get; set; }
        public string content { get; set; }
        public DateTime? dateread { get; set; }
        public DateTime messagesent { get; set; } = DateTime.UtcNow;
    }
}