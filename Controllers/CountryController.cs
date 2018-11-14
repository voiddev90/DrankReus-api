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
        public IActionResult GetTableData(){
            return Ok(db.Country.Select(m => m));
        }
    }
}