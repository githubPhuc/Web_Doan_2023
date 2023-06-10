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
    public class DisplayProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public DisplayProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/DisplayProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayProduct>>> GetDisplayProduct()
        {
          if (_context.DisplayProduct == null)
          {
              return NotFound();
          }
            return await _context.DisplayProduct.ToListAsync();
        }

        // GET: api/DisplayProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayProduct>> GetDisplayProduct(int id)
        {
          if (_context.DisplayProduct == null)
          {
              return NotFound();
          }
            var displayProduct = await _context.DisplayProduct.FindAsync(id);

            if (displayProduct == null)
            {
                return NotFound();
            }

            return displayProduct;
        }

        // PUT: api/DisplayProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisplayProduct(int id, DisplayProduct displayProduct)
        {
            if (id != displayProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(displayProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisplayProductExists(id))
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

        // POST: api/DisplayProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DisplayProduct>> PostDisplayProduct(DisplayProduct displayProduct)
        {
          if (_context.DisplayProduct == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.DisplayProduct'  is null.");
          }
            _context.DisplayProduct.Add(displayProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisplayProduct", new { id = displayProduct.Id }, displayProduct);
        }

        // DELETE: api/DisplayProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisplayProduct(int id)
        {
            if (_context.DisplayProduct == null)
            {
                return NotFound();
            }
            var displayProduct = await _context.DisplayProduct.FindAsync(id);
            if (displayProduct == null)
            {
                return NotFound();
            }

            _context.DisplayProduct.Remove(displayProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DisplayProductExists(int id)
        {
            return (_context.DisplayProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
