using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortConnectionsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public PortConnectionsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? name)
        {
            var data = await (from a in db_.PortConnection
                              where (
                                       (name == null || name == "" || a.Name.ToUpper().Contains(name.ToUpper()))
                                    )
                              select new
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Description = a.Description,
                                  TechnicalData = a.TechnicalData,
                              }).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }

        // POST: api/CategoryProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult> insert(string Name, string Description, string TechnicalData)
        {
            var check = await db_.PortConnection.Where(a => a.Name == Name).FirstOrDefaultAsync();
            if (check != null)
            {
                return Ok(new Response
                {
                    Status = "Failed",
                    Message = " Name Port Connection is exist!"
                });
            }
            else
            {
                var data = new PortConnection()
                {
                    Name = Name,
                    Description = Description,
                    TechnicalData = TechnicalData,
                };
                db_.PortConnection.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Insert Port Connection " + Name + " successfully!" });
            }
        }

        // DELETE: api/CategoryProducts/5
        [HttpPost("delete")]
        public async Task<IActionResult> delete(int id)
        {
            var PortConnection = await db_.PortConnection.FindAsync(id);
            if (PortConnection == null)
            {
                return Ok(new Response
                {
                    Status = "Failed",
                    Message = "Port Connection not exist!"
                });
            }
            db_.PortConnection.Remove(PortConnection);
            await db_.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Delete Port Connection successfully!" });
        }

        private bool PortConnectionExists(int id)
        {
            return (db_.PortConnection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
