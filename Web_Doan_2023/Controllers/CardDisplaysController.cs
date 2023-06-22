using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardDisplaysController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public CardDisplaysController(Web_Doan_2023Context context)
        {
            db_ = context;
        }
        // GET: api/Cities
        [HttpGet]
        public async Task<IActionResult> GetCardDisplay(string? name)
        {
            var data = await (from a in db_.CardDisplay
                              where (
                                       (name == null || name == "" || a.Name.Contains(name))
                                    )
                              select new
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  TechnicalData = a.TechnicalData,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CardDisplay>> PostCardDisplay(string Name,string TechnicalData)
        {
            if (String.IsNullOrEmpty(Name))
            {
                return Ok(new Response { Status = "Failed", Message = "Card Display name is null!" });
            }
            else
            {
                var check = await db_.CardDisplay.Where(a => a.Name == Name).ToListAsync();
                if (check.Count() > 0)
                {
                    return Ok(new Response { Status = "Failed", Message = "Card Display name exists in database!" });
                }
                var data = new CardDisplay()
                {
                    Name = Name,
                    TechnicalData= TechnicalData,
                };
                db_.CardDisplay.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Card Display created successfully!" });
            }
        }

        // DELETE: api/Cities/5
        [HttpPost("DeleteCardDisplay")]
        public async Task<IActionResult> DeleteCardDisplay(int id)
        {
           
            var data = await db_.CardDisplay.FirstOrDefaultAsync(a => a.Id == id);
            if (data != null)
            {
                db_.CardDisplay.Remove(data);
                await db_.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Card Display delete successfully!" });
            }
            return Ok(new Response { Status = "Failed", Message = "Card Display delete failed!" });
        }

        private bool CardDisplayExists(int id)
        {
            return (db_.CardDisplay?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
