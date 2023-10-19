using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength =4)]
        public string password {get; set;}
        public string gender { get; set; }
        public string bio { get; set; }
        public int age { get; set; }
        public string kwonas { get; set; }
        public string created { get; set; }
        public string lastactive { get; set; }
        public string intrests { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        public List<PhotoDTO> photos {get; set;} = new();
    }

}