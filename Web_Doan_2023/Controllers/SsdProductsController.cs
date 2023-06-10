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
    public class SsdProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public SsdProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/SsdProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SsdProduct>>> GetSsdProduct()
        {
          if (_context.SsdProduct == null)
          {
              return NotFound();
          }
            return await _context.SsdProduct.ToListAsync();
        }

        // GET: api/SsdProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SsdProduct>> GetSsdProduct(int id)
        {
          if (_context.SsdProduct == null)
          {
              return NotFound();
          }
            var ssdProduct = await _context.SsdProduct.FindAsync(id);

            if (ssdProduct == null)
            {
                return NotFound();
            }

            return ssdProduct;
        }

        // PUT: api/SsdProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSsdProduct(int id, SsdProduct ssdProduct)
        {
            if (id != ssdProduct.Id)
            {
                return Ok(new Response { Status = "Failed", Message = " Id Ssd exist!" });
            }

            _context.Entry(ssdProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SsdProductExists(id))
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

        // POST: api/SsdProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SsdProduct>> PostSsdProduct(SsdProduct ssdProduct)
        {
            if(ssdProduct.Name ==null|| ssdProduct.Name=="")
            {
                return Ok(new Response { Status = "Failed", Message = "Name ssd not null!" });
            }
            if (_context.SsdProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ssd exist!" });
            }
            var dataCheck = _context.SsdProduct.Where(a => a.Name == ssdProduct.Name&& a.TechnicalData ==ssdProduct.TechnicalData).ToList();
            if (dataCheck.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "Ssd name and Technical Data exist!" });
            }

            _context.SsdProduct.Add(ssdProduct);
            await _context.SaveChangesAsync();

            
            return Ok(new Response { Status = "Success", Message = "Ssd created successfully!" });
        }

        // DELETE: api/SsdProducts/5
        [HttpPost("DeleteSsd")]
        public async Task<IActionResult> DeleteSsdProduct(int id)
        {
            if (_context.SsdProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ssd exist!" });
            }
            var ssdProduct = await _context.SsdProduct.FindAsync(id);
            if (ssdProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ssd not in the database!" });
            }

            _context.SsdProduct.Remove(ssdProduct);
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Ssd delete successfully!" });

        }

        private bool SsdProductExists(int id)
        {
            return (_context.SsdProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
