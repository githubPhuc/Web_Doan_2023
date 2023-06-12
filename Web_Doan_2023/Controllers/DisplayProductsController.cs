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
        [HttpGet("Search")]
        public async Task<IActionResult> GetDisplay(string name,string Techno)
        {
            var result = _context.DisplayProduct.Where(a=>
                        (name == null|| name == ""|| a.Name.Contains(name))&&
                        (Techno == null|| Techno == ""|| a.TechnicalData.Contains(Techno))
                        ).ToList();
            return Ok(result);
        }


        // POST: api/DisplayProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DisplayProduct>> PostDisplayProduct(DisplayProduct data)
        {
            if (data.Name == null || data.Name == "")
            {
                return Ok(new Response { Status = "Failed", Message = "Name display not null!" });
            }
            if (_context.DisplayProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Display exist!" });
            }
            var dataCheck = _context.DisplayProduct.Where(a => a.Name == data.Name && a.TechnicalData == data.TechnicalData).ToList();
            if (dataCheck.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "Display name and Technical Data exist!" });
            }

            _context.DisplayProduct.Add(data);
            await _context.SaveChangesAsync();


            return Ok(new Response { Status = "Success", Message = "Display created successfully!" });
        }

        // DELETE: api/DisplayProducts/5
        [HttpPost("DeleteDisplay")]
        public async Task<IActionResult> DeleteDisplayProduct(int id)
        {
            if (_context.RamProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ram exist!" });
            }
            var ramProduct = await _context.RamProduct.FindAsync(id);
            if (ramProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Ram not in the database!" });
            }

            _context.RamProduct.Remove(ramProduct);
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Ram delete successfully!" });
        }

        private bool DisplayProductExists(int id)
        {
            return (_context.DisplayProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
