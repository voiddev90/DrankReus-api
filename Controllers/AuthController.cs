using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Models;
using DrankReus_api.Data;
using Newtonsoft.Json.Linq;
using System.Text;
using DrankReus_api.Helpers;

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
            if(userExists(newUser.Email)) return StatusCode(409);
            newUser.HashPassword();
            newUser.Admin = false;
            db.Users.Add(newUser);
            db.SaveChanges();
            return StatusCode(201);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] JObject loginDetails)
        {
            string email = loginDetails["email"].ToString();
            string password = loginDetails["password"].ToString();
            if(!userExists(email)) return StatusCode(409);

            User registeredUser = db.Users.Where(u => u.Email == email).First();
            
            if(!registeredUser.PasswordMatch(password)) return StatusCode(409);
            return Ok(TokenHelper.generateToken(registeredUser, DateTime.Now.AddHours(1)));
        }

        private bool userExists(string email)
        {
            bool userExists = db.Users.Any(u => u.Email == email);
            return userExists;
        }
    }
}
