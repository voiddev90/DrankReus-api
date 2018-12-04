using System.Linq;
using DrankReus_api.Data;
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
        public IActionResult GetTableData(
        [FromQuery(Name = "Country")]int[] Ids
        ){
            var result = db.Country.Select(m => m);
            if(Ids.Length != 0){
                result = result.Where(m => Ids.Contains(m.Id));
                return Ok(result);
            }
            else
            return Ok(db.Country.Select(m => m));
        }
    }
}