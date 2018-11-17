using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DrankReus_api.Data;
using DrankReus_api.Models;

namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebshopContext db;

        public UsersController(WebshopContext context) { db = context; }

        // GET api/users/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            User user = GetClaimUser(id);
            if(user == null)
            {
                return Unauthorized();
            }
            return Ok(user.UserData());
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/users/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        private User GetClaimUser(int requestId)
        {
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return (from u in db.Users where u.Email == claimEmail && u.Id == requestId select u).FirstOrDefault();
        }
    }
}
