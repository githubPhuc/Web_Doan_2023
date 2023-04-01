using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_PageController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;
        public User_PageController(Web_Doan_2023Context context)
        {
            _context = context;
           
        }

        // GET: api/User_Page
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User_Page>>> GetUser_Page()
        {
          if (_context.User_Page == null)
          {
              return NotFound();
          }
            return await _context.User_Page.ToListAsync();
        }

        // GET: api/User_Page/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User_Page>> GetUser_Page(int id)
        {
          if (_context.User_Page == null)
          {
              return NotFound();
          }
            var user_Page = await _context.User_Page.FindAsync(id);

            if (user_Page == null)
            {
                return NotFound();
            }

            return user_Page;
        }

        // PUT: api/User_Page/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser_Page(int id, User_Page user_Page)
        {
            if (id != user_Page.Id)
            {
                return BadRequest();
            }

            _context.Entry(user_Page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_PageExists(id))
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

        // POST: api/User_Page
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User_Page>> PostUser_Page([FromForm]User_Page user_Page)
        {
          if (_context.User_Page == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.User_Page'  is null.");
          }
          var check1= _context.Users.Where(a=>a.AccoutType =="Admin").ToList();
            if(check1.Count()>0)
            {
                _context.User_Page.Add(user_Page);
                var data = new User_Page();
                data.IdUsercreate = HttpContext.Session.GetString("userName"); 
                data.Created = DateTime.Now;
                data.IdUserupdate = HttpContext.Session.GetString("userName");
                data.Updated = DateTime.Now;
                _context.User_Page.Update(data);
                await _context.SaveChangesAsync();

                return  Ok(new Response { Status = "Success", Message = "User page created successfully!" });
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "User created successfully!" });
            }
           
        }

        // DELETE: api/User_Page/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser_Page(int id)
        {
            if (_context.User_Page == null)
            {
                return NotFound();
            }
            var user_Page = await _context.User_Page.FindAsync(id);
            if (user_Page == null)
            {
                return NotFound();
            }

            _context.User_Page.Remove(user_Page);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool User_PageExists(int id)
        {
            return (_context.User_Page?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
