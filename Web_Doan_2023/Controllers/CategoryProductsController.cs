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
    public class CategoryProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public CategoryProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/CategoryProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryProduct>>> GetCategoryProduct()
        {
          if (_context.CategoryProduct == null)
          {
              return NotFound();
          }
            return await _context.CategoryProduct.ToListAsync();
        }

        // GET: api/CategoryProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryProduct>> GetCategoryProduct(int id)
        {
          if (_context.CategoryProduct == null)
          {
              return NotFound();
          }
            var categoryProduct = await _context.CategoryProduct.FindAsync(id);

            if (categoryProduct == null)
            {
                return NotFound();
            }

            return categoryProduct;
        }

        // PUT: api/CategoryProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryProduct(int id, CategoryProduct categoryProduct)
        {
            if (id != categoryProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoryProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryProductExists(id))
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

        // POST: api/CategoryProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryProduct>> PostCategoryProduct([FromBody] CategoryProduct categoryProduct)
        {
          if (_context.CategoryProduct == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.CategoryProduct'  is null.");
          }
            _context.CategoryProduct.Add(categoryProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryProduct", new { id = categoryProduct.Id }, categoryProduct);
        }

        // DELETE: api/CategoryProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryProduct(int id)
        {
            if (_context.CategoryProduct == null)
            {
                return NotFound();
            }
            var categoryProduct = await _context.CategoryProduct.FindAsync(id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            var dataProduct = _context.Product.Where(a => a.idCategory == id).ToList();
            if(dataProduct.Count > 0) { return Ok(new Response { Status = "Failed", Message = "The product exists in the database!Unable to delete category product" }); }
            _context.CategoryProduct.Remove(categoryProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryProductExists(int id)
        {
            return (_context.CategoryProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
