using Backend.Models;
using Backend.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Fragenkatalog : ControllerBase
    {
        private AppDbContext _ctx;

        public Fragenkatalog(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> GetFragenpool([FromQuery] int id)
        {
            if (id == 0) 
            { id = 1; }

            var Fragen = await _ctx.Fragen.Where(a => a.DateiId == id).ToListAsync();

            List<HCFA> restul = new List<HCFA>();

            foreach (var f in Fragen)
            {
                if (f.hint == null)
                { f.hint = ""; };

                var Antworten = await _ctx.Antworten.Where(a => a.FragenId == f.Id).ToListAsync();
                List<Dictionary<string, object>> Ant = new List<Dictionary<string, object>>();

                foreach (var antwort in Antworten)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict["answerText"] = antwort.AnswerText;
                    dict["isCorecct"] = antwort.IsCorrect;
                    Ant.Add(dict);
                }

                var HCFA = new HCFA 
                {
                    type = f.Type,
                    questionText = f.QuestionText,
                    hint = f.hint,
                    Antworten = Ant,
                };
                
                restul.Add(HCFA);
            }

            return Ok(restul);
        }

    }
}
