using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Entitites;

namespace API.Controllers
{
    public class BuggyController:BaseApiController
    {
        private readonly IuserRepository _context;

        public BuggyController( IuserRepository context)
        {
          _context=context;
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
           var thing=_context.Users.Find(-1);
            if(thing==null) return NotFound();
            return  Ok(thing);
        }

        
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
           var thing=_context.Users.Find(-1);

           var thingToReturn=thing.ToString();
           return thingToReturn;
        }

        [HttpGet("bad-Request")]
        public ActionResult<string> badRequest()
        {
           return BadRequest("This was not a good request");
        }
        
    }
}