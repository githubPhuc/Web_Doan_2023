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
    public class ColorProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public ColorProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/ColorProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorProduct>>> GetColorProduct()
        {
          if (_context.ColorProduct == null)
          {
              return NotFound();
          }
            return await _context.ColorProduct.ToListAsync();
        }

        // GET: api/ColorProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorProduct>> GetColorProduct(int id)
        {
          if (_context.ColorProduct == null)
          {
              return NotFound();
          }
            var colorProduct = await _context.ColorProduct.FindAsync(id);

            if (colorProduct == null)
            {
                return NotFound();
            }

            return colorProduct;
        }

        // PUT: api/ColorProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColorProduct(int id, ColorProduct colorProduct)
        {
            if (id != colorProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(colorProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorProductExists(id))
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

        // POST: api/ColorProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ColorProduct>> PostColorProduct(ColorProduct colorProduct)
        {
          if (_context.ColorProduct == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.ColorProduct'  is null.");
          }
            _context.ColorProduct.Add(colorProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColorProduct", new { id = colorProduct.Id }, colorProduct);
        }

        // DELETE: api/ColorProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColorProduct(int id)
        {
            if (_context.ColorProduct == null)
            {
                return NotFound();
            }
            var colorProduct = await _context.ColorProduct.FindAsync(id);
            if (colorProduct == null)
            {
                return NotFound();
            }

            _context.ColorProduct.Remove(colorProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColorProductExists(int id)
        {
            return (_context.ColorProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
