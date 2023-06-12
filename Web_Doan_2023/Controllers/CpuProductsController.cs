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

      

        // POST: api/CpuProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CpuProduct>> PostCpuProduct(CpuProduct data)
        {
            if (data.Name == null || data.Name == "")
            {
                return Ok(new Response { Status = "Failed", Message = "Name Cpu not null!" });
            }
            if (_context.CpuProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Cpu exist!" });
            }
            var dataCheck = _context.CpuProduct.Where(a => a.Name == data.Name && a.TechnicalData == data.TechnicalData).ToList();
            if (dataCheck.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "Cpu name and Technical Data exist!" });
            }

            _context.CpuProduct.Add(data);
            await _context.SaveChangesAsync();


            return Ok(new Response { Status = "Success", Message = "Cpu created successfully!" });
        }

        // DELETE: api/CpuProducts/5
        [HttpPost("DeleteCpu")]
        public async Task<IActionResult> DeleteCpuProduct(int id)
        {
            if (_context.CpuProduct == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Cpu exist!" });
            }
            var data = await _context.CpuProduct.FindAsync(id);
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Cpu not in the database!" });
            }

            _context.CpuProduct.Remove(data);
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Cpu delete successfully!" });
        }

        private bool CpuProductExists(int id)
        {
            return (_context.CpuProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
