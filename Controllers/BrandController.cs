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
    public class BrandController : Controller
    {
        private readonly WebshopContext db;
        public BrandController(WebshopContext context){
            db = context;
        }

        [HttpGet("{Id}")]
        public ActionResult GetBrand(int id){
            return Ok(db.Brand.Select(m => m)
            .Where(m=>m.Id == id).ToArray());
        }

        [HttpGet]
        public IActionResult GetTableData(){
            return Ok(db.Brand.Select(m => m));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBrand([FromBody] Brand newBrand)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Brand.Add(newBrand);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBrand(int id, [FromBody] Brand updateBrand)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Brand brand = await db.Brand.FindAsync(id);

            if(brand == null)
            {
                return NotFound();
            }

            brand.Name = updateBrand.Name;
            db.Brand.Update(brand);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}