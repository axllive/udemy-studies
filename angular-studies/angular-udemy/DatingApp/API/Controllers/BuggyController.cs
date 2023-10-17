using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPiController
    {
        private readonly DBContext _context;
        public BuggyController(DBContext dBContext)
        {
            _context = dBContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            AppUser usr = _context.Users.Find(-1);

            if (usr == null) return NotFound();
            else return usr;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            AppUser usr = _context.Users.Find(-1);
            string err = usr.ToString();
            return err;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("I wanna do bad things to you....");
        }
    }
}