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


        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {
            var Users = (from u in db.Users
                         select new UserInformation()
                         {
                             Id = u.Id,
                             Email = u.Email,
                             FirstName = u.FirstName,
                             Prefix = u.Prefix,
                             LastName = u.Prefix,
                             Street = u.Street,
                             BuildingNumber = u.BuildingNumber,
                             PostalCode = u.PostalCode,
                             Area = u.Area,
                             DiscountPoints = u.DiscountPoints,
                             Admin = u.Admin
                         }).ToArray();
            return Ok(Users);
        }

        // GET api/users/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            User currentUser = GetClaimUser();

            if (currentUser != null && (currentUser.Admin || currentUser.Id == id))
            {
                User requestedUser = db.Users.Find(id);
                return Ok(requestedUser.UserData());
            }
            return Unauthorized();
        }

        // POST api/users
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserInformation newUserInfo)
        {
            if (userExists(newUserInfo.Email)) return StatusCode(409, "Gebruiker bestaat al.");
            User newUser = new User()
            {
                Email = newUserInfo.Email,
                Password = UserHelper.HashPassword(newUserInfo.Password),
                FirstName = newUserInfo.FirstName,
                Prefix = newUserInfo.Prefix,
                LastName = newUserInfo.LastName,
                Street = newUserInfo.Street,
                BuildingNumber = newUserInfo.BuildingNumber,
                PostalCode = newUserInfo.PostalCode,
                Area = newUserInfo.Area,
                DiscountPoints = 0,
                Admin = false
            };
            User currentUser = GetClaimUser();
            if (currentUser != null && currentUser.Admin)
            {
                newUser.Admin = newUserInfo.Admin.HasValue ? newUserInfo.Admin.Value : false;
                newUser.DiscountPoints = newUserInfo.DiscountPoints.HasValue ? newUserInfo.DiscountPoints.Value : 0;
            }
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
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
            if (!userExists(email)) return StatusCode(409, "Gebruiker bestaat niet");

            User registeredUser = db.Users.Where(u => u.Email == email).First();

            if (!UserHelper.PasswordMatch(registeredUser.Password, password)) return StatusCode(409, "Verkeerd wachtwoord");
            return Ok(TokenHelper.generateToken(registeredUser, DateTime.Now.AddHours(1)));
        }

        // PUT api/users/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UserInformation userInformation)
        {
            User currentUser = GetClaimUser();
            User user = await db.Users.FindAsync(id);
            if (currentUser == null || user == null)
            {
                return NotFound();
            }

            if (currentUser.Admin || currentUser.Id == user.Id)
            {
                if (user.Email != userInformation.Email)
                {
                    if (userExists(userInformation.Email)) return StatusCode(409, "Er is al een gebruiker geregistreerd op dit email");
                }
                if (userInformation.Password != null)
                {
                    user.Password = UserHelper.HashPassword(userInformation.Password);
                }
                user.FirstName = userInformation.Email;
                user.Prefix = userInformation.Prefix;
                user.LastName = userInformation.LastName;
                user.Street = userInformation.Street;
                user.BuildingNumber = userInformation.BuildingNumber;
                user.PostalCode = userInformation.PostalCode;
                user.Area = userInformation.Area;
                if (currentUser.Admin)
                {
                    user.DiscountPoints = userInformation.DiscountPoints.HasValue ? userInformation.DiscountPoints.Value : user.DiscountPoints;
                    user.Admin = userInformation.Admin.HasValue ? userInformation.Admin.Value : user.Admin;
                }
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok();
        }

        private bool userExists(string email)
        {
            bool userExists = db.Users.Any(u => u.Email == email);
            return userExists;
        }

        private User GetClaimUser()
        {
            if (User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
            }
            else return null;
        }
    }
}
