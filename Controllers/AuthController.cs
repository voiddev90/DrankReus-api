using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Models;
using DrankReus_api.Data;
using Newtonsoft.Json.Linq;

namespace DrankReus_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WebshopContext db;

        public AuthController(WebshopContext context) {db = context;}

        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] User newUser)
        {
            bool userExists = db.Users.Any(u => u.Email == newUser.Email);
            if(userExists) return StatusCode(409);
            newUser.HashPassword();
            newUser.Admin = false;
            db.Users.Add(newUser);
            db.SaveChanges();
            return StatusCode(201);
        }
    }
}
