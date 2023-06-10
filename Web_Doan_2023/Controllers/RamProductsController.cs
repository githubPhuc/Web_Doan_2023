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
    public class RamProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public RamProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/RamProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RamProduct>>> GetRamProduct()
        {
          if (_context.RamProduct == null)
          {
              return NotFound();
          }
            return await _context.RamProduct.ToListAsync();
        }

        // GET: api/RamProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RamProduct>> GetRamProduct(int id)
        {
          if (_context.RamProduct == null)
          {
              return NotFound();
          }
            var ramProduct = await _context.RamProduct.FindAsync(id);

            if (ramProduct == null)
            {
                return NotFound();
            }

            return ramProduct;
        }

        // PUT: api/RamProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRamProduct(int id, RamProduct ramProduct)
        {
            if (id != ramProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(ramProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RamProductExists(id))
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

        // POST: api/RamProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RamProduct>> PostRamProduct(RamProduct ramProduct)
        {
          

            if (ramProduct.nameRam == null || ramProduct.nameRam == "")
            {
                return Ok(new Response { Status = "Failed", Message = "Name Ram not null!" });
            }
            if (_context.RamProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ram exist!" });
            }
            var dataCheck = _context.RamProduct.Where(a => a.nameRam == ramProduct.nameRam && a.TechnicalData == ramProduct.TechnicalData).ToList();
            if (dataCheck.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "Ram name and Technical Data exist!" });
            }

            _context.RamProduct.Add(ramProduct);
            await _context.SaveChangesAsync();


            return Ok(new Response { Status = "Success", Message = "Ram created successfully!" });
        }

        // DELETE: api/RamProducts/5
        [HttpPost("DeleteRam")]
        public async Task<IActionResult> DeleteRamProduct(int id)
        {
            if (_context.RamProduct == null)
            {
                return NotFound();
            }
            var ramProduct = await _context.RamProduct.FindAsync(id);
            if (ramProduct == null)
            {
                return NotFound();
            }

            _context.RamProduct.Remove(ramProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RamProductExists(int id)
        {
            return (_context.RamProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
