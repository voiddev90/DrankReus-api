using System.Linq;
using System.Threading.Tasks;
using DrankReus_api.Data;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly WebshopContext db;
        public CountryController(WebshopContext context){
            db = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetTableData(){
            return Ok(db.Country.Select(m => m));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCountry([FromBody] Country newCountry)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Country.Add(newCountry);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCountry(int id, [FromBody] Country UpdateCountry)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Country country = await db.Country.FindAsync(id);

            if(country == null)
            {
                return NotFound();
            }

            country.Name = UpdateCountry.Name;
            db.Country.Update(country);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}