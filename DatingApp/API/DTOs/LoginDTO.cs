using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public string token {get; set;}
    }
}