using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DrankReus_api.Data;
using DrankReus_api.Models;
using DrankReus_api.Helpers;

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
            User currentUser = GetClaimUser();

            if(currentUser != null && (currentUser.Admin || currentUser.Id == id))
            {
                User requestedUser = db.Users.Find(id);
                return Ok(requestedUser.UserData());
            }
            return Unauthorized();
        }

        // POST api/users
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post([FromBody] User newUser)
        {
            if(userExists(newUser.Email)) return StatusCode(409, "Gebruiker bestaat al.");
            newUser.HashPassword();
            newUser.Admin = false;
            db.Users.Add(newUser);
            db.SaveChanges();
            return StatusCode(201);
        }

        // POST api/users/login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] JObject loginDetails)
        {
            string email = loginDetails["email"].ToString();
            string password = loginDetails["password"].ToString();
            if(!userExists(email)) return StatusCode(409, "Gebruiker bestaat niet");

            User registeredUser = db.Users.Where(u => u.Email == email).First();
            
            if(!registeredUser.PasswordMatch(password)) return StatusCode(409, "Verkeerd wachtwoord");
            return Ok(TokenHelper.generateToken(registeredUser, DateTime.Now.AddHours(1)));
        }

        // PUT api/users/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        private bool userExists(string email)
        {
            bool userExists = db.Users.Any(u => u.Email == email);
            return userExists;
        }

        private User GetClaimUser()
        {
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
        }
    }
}
