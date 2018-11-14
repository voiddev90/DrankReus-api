using System.Linq;
using DrankReus_api.Data;
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

        [HttpGet]
        [Route("")]
        public IActionResult GetTableData(){
            return Ok(db.Brand.Select(m => m));
        }


    }
}