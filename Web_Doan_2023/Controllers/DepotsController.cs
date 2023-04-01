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
        public async Task<IActionResult> PutDepot(int id, Depot depot)
        {
            if (id != depot.Id)
            {
                return BadRequest();
            }

            _context.Entry(depot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
            _context.Depot.Add(depot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepot", new { id = depot.Id }, depot);
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
