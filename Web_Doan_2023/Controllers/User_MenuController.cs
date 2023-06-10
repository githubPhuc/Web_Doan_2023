using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_MenuController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public User_MenuController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/User_Menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User_Menu>>> GetUser_Menu()
        {
          if (_context.User_Menu == null)
          {
              return NotFound();
          }
            return await _context.User_Menu.ToListAsync();
        }
        [HttpGet]
        [Route("Load_User_Menu")]
        public async Task<ActionResult> Load_User_Menu(string userName)
        {
            var dataCheck = _context.Users.Where(a => a.UserName == userName&& a.IsLocked==false && a.AccoutType=="Admin").FirstOrDefault();
            if (dataCheck == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Account not Data!" });
            }
            var data = (
                            from user in _context.User_Menu
                            join Menu in _context.Menu on user.MenuId equals Menu.Id
                            where user.UserId == userName
                            orderby Menu.oder_no
                            select new
                            {
                                menuName = Menu.Menu_Name,
                                menuId = Menu.Id,
                                icon = Menu.Icon,
                                userName = user.UserId
                            }

                        ).ToArray();
            return Ok(new
            {
                menu = data,
                count = data.Count()
            });
        }

        // GET: api/User_Menu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User_Menu>> GetUser_Menu(int id)
        {
          if (_context.User_Menu == null)
          {
              return NotFound();
          }
            var user_Menu = await _context.User_Menu.FindAsync(id);

            if (user_Menu == null)
            {
                return NotFound();
            }

            return user_Menu;
        }

        // PUT: api/User_Menu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser_Menu(int id, User_Menu user_Menu)
        {
            if (id != user_Menu.Id)
            {
                return BadRequest();
            }

            _context.Entry(user_Menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_MenuExists(id))
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

        // POST: api/User_Menu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User_Menu>> PostUser_Menu(User_Menu user_Menu)
        {
          if (_context.User_Menu == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.User_Menu'  is null.");
          }
            _context.User_Menu.Add(user_Menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser_Menu", new { id = user_Menu.Id }, user_Menu);
        }

        // DELETE: api/User_Menu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser_Menu(int id)
        {
            if (_context.User_Menu == null)
            {
                return NotFound();
            }
            var user_Menu = await _context.User_Menu.FindAsync(id);
            if (user_Menu == null)
            {
                return NotFound();
            }

            _context.User_Menu.Remove(user_Menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool User_MenuExists(int id)
        {
            return (_context.User_Menu?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
