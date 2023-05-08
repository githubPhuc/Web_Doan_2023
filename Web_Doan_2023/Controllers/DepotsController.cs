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
    public class DepotsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public DepotsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/Depots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Depot>>> GetDepot()
        {
          if (_context.Depot == null)
          {
              return NotFound();
          }
            return await _context.Depot.ToListAsync();
        }

        // GET: api/Depots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Depot>> GetDepot(int id)
        {
          if (_context.Depot == null)
          {
              return NotFound();
          }
            var depot = await _context.Depot.FindAsync(id);

            if (depot == null)
            {
                return NotFound();
            }

            return depot;
        }

        // PUT: api/Depots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepot(int id, Depot depot)// Lỗi rồi
        {
            if (id != depot.Id)
            {
                return BadRequest();
            }
            var dataDepots = new Depot();
               dataDepots.codeDepot = depot.codeDepot.ToUpper();
                dataDepots.nameDepot = depot.nameDepot;
                dataDepots.Phone = depot.Phone;
                dataDepots.Location = depot.Location;
                dataDepots.status = depot.status;
                dataDepots.storekeepers = depot.storekeepers;
            
            _context.Entry(dataDepots).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepotExists(id))
                {
                    return Ok(new Response { Status = "Failed", Message = "Update Depots failed!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                status = 200,
                id = dataDepots.Id,
            });
        }

        // POST: api/Depots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Depot>> PostDepot(Depot depot)
        {
            if (_context.Depot == null)
            {
                return Problem("Entity set 'Web_Doan_2023Context.Depot'  is null.");
            }
            string code = depot.codeDepot.ToUpper();
            if (code.Length != 3)
            {
                return Ok(new Response { Status = "Failed", Message = "Code depots in three characters long" });

            }
            var dataDepots = new Depot()
            {                
                codeDepot = depot.codeDepot.ToUpper(),
                nameDepot = depot.nameDepot,
                Phone = depot.Phone,
                Location = depot.Location,
                status = true,
                storekeepers = depot.storekeepers,
            };
            _context.Depot.Add(dataDepots);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = 200,
                id = dataDepots.Id,
            });
        }

        // DELETE: api/Depots/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepot(int id)
        {
            if (_context.Depot == null)
            {
                return NotFound();
            }
            var depot = await _context.Depot.FindAsync(id);
            if (depot == null)
            {
                return NotFound();
            }

            _context.Depot.Remove(depot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepotExists(int id)
        {
            return (_context.Depot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
