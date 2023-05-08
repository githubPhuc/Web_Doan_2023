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
    public class BillOfSalesController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public BillOfSalesController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/BillOfSales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillOfSale>>> GetBillOfSale()
        {
          if (_context.BillOfSale == null)
          {
              return NotFound();
          }
            return await _context.BillOfSale.ToListAsync();
        }

        // GET: api/BillOfSales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillOfSale>> GetBillOfSale(int id)
        {
          if (_context.BillOfSale == null)
          {
              return NotFound();
          }
            var billOfSale = await _context.BillOfSale.FindAsync(id);

            if (billOfSale == null)
            {
                return NotFound();
            }

            return billOfSale;
        }

        // PUT: api/BillOfSales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillOfSale(int id, BillOfSale billOfSale)
        {
            if (id != billOfSale.Id)
            {
                return BadRequest();
            }

            _context.Entry(billOfSale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillOfSaleExists(id))
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

        // POST: api/BillOfSales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BillOfSale>> PostBillOfSale(BillOfSale billOfSale)
        {
          if (_context.BillOfSale == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.BillOfSale'  is null.");
          }
            _context.BillOfSale.Add(billOfSale);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillOfSale", new { id = billOfSale.Id }, billOfSale);
        }

        // DELETE: api/BillOfSales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillOfSale(int id)
        {
            if (_context.BillOfSale == null)
            {
                return NotFound();
            }
            var billOfSale = await _context.BillOfSale.FindAsync(id);
            if (billOfSale == null)
            {
                return NotFound();
            }

            _context.BillOfSale.Remove(billOfSale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillOfSaleExists(int id)
        {
            return (_context.BillOfSale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
