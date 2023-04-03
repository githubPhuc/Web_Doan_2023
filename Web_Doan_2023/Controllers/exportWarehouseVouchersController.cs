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
    public class exportWarehouseVouchersController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public exportWarehouseVouchersController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/exportWarehouseVouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<exportWarehouseVouchers>>> GetexportWarehouseVouchers()
        {
          if (_context.exportWarehouseVouchers == null)
          {
              return NotFound();
          }
            return await _context.exportWarehouseVouchers.ToListAsync();
        }

        // GET: api/exportWarehouseVouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<exportWarehouseVouchers>> GetexportWarehouseVouchers(int id)
        {
          if (_context.exportWarehouseVouchers == null)
          {
              return NotFound();
          }
            var exportWarehouseVouchers = await _context.exportWarehouseVouchers.FindAsync(id);

            if (exportWarehouseVouchers == null)
            {
                return NotFound();
            }

            return exportWarehouseVouchers;
        }

        // PUT: api/exportWarehouseVouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutexportWarehouseVouchers(int id, exportWarehouseVouchers exportWarehouseVouchers)
        {
            if (id != exportWarehouseVouchers.Id)
            {
                return BadRequest();
            }

            _context.Entry(exportWarehouseVouchers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!exportWarehouseVouchersExists(id))
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

        // POST: api/exportWarehouseVouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<exportWarehouseVouchers>> PostexportWarehouseVouchers(exportWarehouseVouchers exportWarehouseVouchers)
        {
          if (_context.exportWarehouseVouchers == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.exportWarehouseVouchers'  is null.");
          }
            _context.exportWarehouseVouchers.Add(exportWarehouseVouchers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetexportWarehouseVouchers", new { id = exportWarehouseVouchers.Id }, exportWarehouseVouchers);
        }

        // DELETE: api/exportWarehouseVouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteexportWarehouseVouchers(int id)
        {
            if (_context.exportWarehouseVouchers == null)
            {
                return NotFound();
            }
            var exportWarehouseVouchers = await _context.exportWarehouseVouchers.FindAsync(id);
            if (exportWarehouseVouchers == null)
            {
                return NotFound();
            }

            _context.exportWarehouseVouchers.Remove(exportWarehouseVouchers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool exportWarehouseVouchersExists(int id)
        {
            return (_context.exportWarehouseVouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
