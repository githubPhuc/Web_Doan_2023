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
        private readonly Web_Doan_2023Context _context;

        public PortConnectionsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/PortConnections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortConnection>>> GetPortConnection()
        {
          if (_context.PortConnection == null)
          {
              return NotFound();
          }
            return await _context.PortConnection.ToListAsync();
        }

        // GET: api/PortConnections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortConnection>> GetPortConnection(int id)
        {
          if (_context.PortConnection == null)
          {
              return NotFound();
          }
            var portConnection = await _context.PortConnection.FindAsync(id);

            if (portConnection == null)
            {
                return NotFound();
            }

            return portConnection;
        }

        // PUT: api/PortConnections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortConnection(int id, PortConnection portConnection)
        {
            if (id != portConnection.Id)
            {
                return BadRequest();
            }

            _context.Entry(portConnection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortConnectionExists(id))
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

        // POST: api/PortConnections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PortConnection>> PostPortConnection(PortConnection portConnection)
        {
          if (_context.PortConnection == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.PortConnection'  is null.");
          }
            _context.PortConnection.Add(portConnection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPortConnection", new { id = portConnection.Id }, portConnection);
        }

        // DELETE: api/PortConnections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortConnection(int id)
        {
            if (_context.PortConnection == null)
            {
                return NotFound();
            }
            var portConnection = await _context.PortConnection.FindAsync(id);
            if (portConnection == null)
            {
                return NotFound();
            }

            _context.PortConnection.Remove(portConnection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PortConnectionExists(int id)
        {
            return (_context.PortConnection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
