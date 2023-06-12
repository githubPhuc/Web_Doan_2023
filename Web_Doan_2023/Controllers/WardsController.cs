using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public WardsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/Wards
        [HttpGet]
        public async Task<IActionResult> GetWards(string ?name)
        {
            var data = await (from a in db_.Wards
                              join b in db_.District on a.IdDistrict equals b.Id
                              join c in db_.City on a.IdCity equals c.Id
                              where (name == null || name == "" || a.NameWards.Contains(name))
                              select new
                              {
                                  Id = a.Id,
                                  NameWards = a.NameWards,
                                  NameDistrict = b.NameDistrict,
                                  NameCity = c.NameCity,
                                  Status = a.Status,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }

        // POST: api/Wards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wards>> PostWards(string NameWards,int IdDistrict,int IdCity)
        {
            if (String.IsNullOrEmpty(NameWards))
            {
                return Ok(new Response { Status = "Failed", Message = "City name is null!" });
            }
            else
            {
                var check = await db_.Wards.Where(a => a.NameWards == NameWards && a.IdDistrict == IdDistrict && a.IdCity ==IdCity).ToListAsync();
                if (check.Count() > 0)
                {
                    return Ok(new Response { Status = "Failed", Message = "Wards name exists in database!" });
                }
                var data = new Wards()
                {
                    IdCity = IdCity,
                    IdDistrict = IdDistrict,
                    NameWards = NameWards,
                    Status = true,
                };
                db_.Wards.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "District created successfully!" });
            }
        }

        // DELETE: api/Wards/5
        [HttpPost("DeleteWards")]
        public async Task<IActionResult> DeleteWards(int id)
        {
          
            var data = await db_.Wards.FirstOrDefaultAsync(a => a.Id == id);
            if (data != null)
            {
                db_.Wards.Remove(data);
                await db_.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Wards delete successfully!" });
            }
            return Ok(new Response { Status = "Failed", Message = "Wards delete failed!" });
           
        }

        private bool WardsExists(int id)
        {
            return (db_.Wards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
