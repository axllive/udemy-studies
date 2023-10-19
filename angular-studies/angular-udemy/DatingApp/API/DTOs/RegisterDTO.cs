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

        public List<PhotoDTO> photos {get; set;} = new();
    }

}