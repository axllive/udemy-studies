using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ChatedMemberDTO
    {
        public string username {get; set;}
        public string knownas { get; set; }
        public DateTime lastactive { get; set; }
        public string photourl { get; set; }

    }
}