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

        // POST: api/ColorProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult> insert(ColorProduct colorProduct)
        {
          var check  = _context.ColorProduct.Where(a=>a.code==colorProduct.code&& a.Name==colorProduct.Name).FirstOrDefault();
            if (check == null)
            {
                var data = new ColorProduct()
                {
                    code = colorProduct.code,
                    Name = colorProduct.Name,
                };
                _context.ColorProduct.Add(data);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Insert color product " + colorProduct.Name + " successfully!" });
            }
            else { return Ok(new Response { Status = "Failed", Message = "Color is exist!" }); }
        }

        // DELETE: api/ColorProducts/5
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.ColorProduct.Where(a=>a.Id==id).FirstOrDefaultAsync();
            if(data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Not data!" });
            }
            else
            {
                _context.ColorProduct.Remove(data);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Delete color successfully!" });
            }
        }

        private bool ColorProductExists(int id)
        {
            return (_context.ColorProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
