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
    public class MenusController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public MenusController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
        {
          if (_context.Menu == null)
          {
              return NotFound();
          }
            return await _context.Menu.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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

        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            var dataCheck = _context.Menu.Where(a=>a.Menu_Name== menu.Menu_Name).ToList();  
            if(dataCheck.Count()>0)
            {
                return Ok(new Response { Status = "Failed", Message = "Menu name exist!" });
            }    
              if (_context.Menu == null)
              {
                  return Ok(new Response { Status = "Failed", Message = "Menu exist!" });
            }

            _context.Menu.Add(menu);        
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Menu created successfully!" });
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
           
            if (_context.Menu == null)
            {
                return NotFound();
            }
            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuExists(int id)
        {
            return (_context.Menu?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
