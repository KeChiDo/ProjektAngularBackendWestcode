using Backend.Models.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Themen : ControllerBase
    {
        private AppDbContext _ctx;

        public Themen(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        public async Task<IActionResult> GetThemen()
        {
            var Datein = await _ctx.Datei.ToListAsync();

            List<HCTH> result = new List<HCTH>();

            foreach (var date in Datein)
            {
                var HCTH = new HCTH
                {
                    id = date.Id,
                    Thema = date.Name,
                };
                result.Add(HCTH);
            }
            return Ok(result);
        }
    }
}
