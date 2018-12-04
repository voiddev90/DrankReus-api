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
        public IActionResult GetTableData(
        [FromQuery(Name = "Brand")]int[] Ids
        ){
            var result = db.Brand.Select(m => m);
            if(Ids.Length != 0){
                result = result.Where(m => Ids.Contains(m.Id));
                return Ok(result);
            }
            else
            return Ok(db.Brand.Select(m => m));
        }
    }
}