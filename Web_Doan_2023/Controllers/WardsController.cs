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
    public class WardsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public WardsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/Wards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wards>>> GetWards()
        {
          if (_context.Wards == null)
          {
              return NotFound();
          }
            return await _context.Wards.ToListAsync();
        }

        // GET: api/Wards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wards>> GetWards(int id)
        {
          if (_context.Wards == null)
          {
              return NotFound();
          }
            var wards = await _context.Wards.FindAsync(id);

            if (wards == null)
            {
                return NotFound();
            }

            return wards;
        }

        // PUT: api/Wards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWards(int id, Wards wards)
        {
            if (id != wards.Id)
            {
                return BadRequest();
            }

            _context.Entry(wards).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WardsExists(id))
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

        // POST: api/Wards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wards>> PostWards(Wards wards)
        {
          if (_context.Wards == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.Wards'  is null.");
          }
            _context.Wards.Add(wards);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWards", new { id = wards.Id }, wards);
        }

        // DELETE: api/Wards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWards(int id)
        {
            if (_context.Wards == null)
            {
                return NotFound();
            }
            var wards = await _context.Wards.FindAsync(id);
            if (wards == null)
            {
                return NotFound();
            }

            _context.Wards.Remove(wards);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WardsExists(int id)
        {
            return (_context.Wards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
