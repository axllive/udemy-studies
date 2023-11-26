using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public AppUser SoucerUser { get; set; }
        public int SoucerUserId { get; set; }
        public AppUser TargetUser { get; set; }
        public int TargetUserId { get; set; }

    }
}