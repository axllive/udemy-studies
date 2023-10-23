using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PhotoDTO
    {
        public int id { get; set; }
        public string url { get; set; }
        public bool ismain { get; set; }
    }
}