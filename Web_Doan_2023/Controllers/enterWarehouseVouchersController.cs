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
    public class enterWarehouseVouchersController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public enterWarehouseVouchersController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/enterWarehouseVouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<enterWarehouseVouchers>>> GetenterWarehouseVouchers()
        {
          if (_context.enterWarehouseVouchers == null)
          {
              return NotFound();
          }
            return await _context.enterWarehouseVouchers.ToListAsync();
        }

        // GET: api/enterWarehouseVouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<enterWarehouseVouchers>> GetenterWarehouseVouchers(int id)
        {
          if (_context.enterWarehouseVouchers == null)
          {
              return NotFound();
          }
            var enterWarehouseVouchers = await _context.enterWarehouseVouchers.FindAsync(id);

            if (enterWarehouseVouchers == null)
            {
                return NotFound();
            }

            return enterWarehouseVouchers;
        }

        // PUT: api/enterWarehouseVouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutenterWarehouseVouchers(int id, enterWarehouseVouchers enterWarehouseVouchers)
        {
            if (id != enterWarehouseVouchers.Id)
            {
                return BadRequest();
            }

            _context.Entry(enterWarehouseVouchers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!enterWarehouseVouchersExists(id))
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

        // POST: api/enterWarehouseVouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<enterWarehouseVouchers>> PostenterWarehouseVouchers(enterWarehouseVouchers enterWarehouseVouchers)
        {
          if (_context.enterWarehouseVouchers == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.enterWarehouseVouchers'  is null.");
          }
            _context.enterWarehouseVouchers.Add(enterWarehouseVouchers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetenterWarehouseVouchers", new { id = enterWarehouseVouchers.Id }, enterWarehouseVouchers);
        }

        // DELETE: api/enterWarehouseVouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteenterWarehouseVouchers(int id)
        {
            if (_context.enterWarehouseVouchers == null)
            {
                return NotFound();
            }
            var enterWarehouseVouchers = await _context.enterWarehouseVouchers.FindAsync(id);
            if (enterWarehouseVouchers == null)
            {
                return NotFound();
            }

            _context.enterWarehouseVouchers.Remove(enterWarehouseVouchers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool enterWarehouseVouchersExists(int id)
        {
            return (_context.enterWarehouseVouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
