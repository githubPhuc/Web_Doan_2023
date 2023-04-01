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
    public class PagesController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public PagesController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/Pages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> GetPage()
        {
          if (_context.Page == null)
          {
              return NotFound();
          }
            return await _context.Page.ToListAsync();
        }

        // GET: api/Pages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Page>> GetPage(int id)
        {
          if (_context.Page == null)
          {
              return NotFound();
          }
            var page = await _context.Page.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return page;
        }

        // PUT: api/Pages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage(int id, Page page)
        {
            if (id != page.Id)
            {
                return BadRequest();
            }

            _context.Entry(page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id))
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

        // POST: api/Pages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Page>> PostPage(Page page)
        {
          if (_context.Page == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.Page'  is null.");
          }
            _context.Page.Add(page);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPage", new { id = page.Id }, page);
        }

        // DELETE: api/Pages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            if (_context.Page == null)
            {
                return NotFound();
            }
            var page = await _context.Page.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            _context.Page.Remove(page);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PageExists(int id)
        {
            return (_context.Page?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
