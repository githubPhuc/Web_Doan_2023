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
    public class CpuProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public CpuProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/CpuProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CpuProduct>>> GetCpuProduct()
        {
          if (_context.CpuProduct == null)
          {
              return NotFound();
          }
            return await _context.CpuProduct.ToListAsync();
        }

        // GET: api/CpuProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CpuProduct>> GetCpuProduct(int id)
        {
          if (_context.CpuProduct == null)
          {
              return NotFound();
          }
            var cpuProduct = await _context.CpuProduct.FindAsync(id);

            if (cpuProduct == null)
            {
                return NotFound();
            }

            return cpuProduct;
        }

        // PUT: api/CpuProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCpuProduct(int id, CpuProduct cpuProduct)
        {
            if (id != cpuProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(cpuProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CpuProductExists(id))
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

        // POST: api/CpuProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CpuProduct>> PostCpuProduct(CpuProduct cpuProduct)
        {
          if (_context.CpuProduct == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.CpuProduct'  is null.");
          }
            _context.CpuProduct.Add(cpuProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCpuProduct", new { id = cpuProduct.Id }, cpuProduct);
        }

        // DELETE: api/CpuProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCpuProduct(int id)
        {
            if (_context.CpuProduct == null)
            {
                return NotFound();
            }
            var cpuProduct = await _context.CpuProduct.FindAsync(id);
            if (cpuProduct == null)
            {
                return NotFound();
            }

            _context.CpuProduct.Remove(cpuProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CpuProductExists(int id)
        {
            return (_context.CpuProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
