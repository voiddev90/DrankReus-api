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
    public class CategoryController : Controller
    {
        private readonly WebshopContext db;
        public CategoryController(WebshopContext context){
            db = context;
        }

        [HttpGet]
        public IActionResult GetTableData(){
            return Ok(db.Category.Select(m => m));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCategory([FromBody] Category newCategory)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Category.Add(newCategory);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] Category UpdateCategory)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = await db.Category.FindAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            category.Name = UpdateCategory.Name;
            db.Category.Update(category);
            await db.SaveChangesAsync();
            return Ok();
        }



    }
}