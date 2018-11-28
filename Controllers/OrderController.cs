using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Data;

namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

      private readonly WebshopContext db;
      public OrderController(WebshopContext context) {db = context;}

    //   [HttpGet]
    //     public ActionResult Get()
    //     {
    //         // var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value; <--- TO GET THE EMAIL FROM THE USER THAT IS IN THE TOKEN
    //         // return Ok(new String[] { "value1", "value2" });
    //     }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
