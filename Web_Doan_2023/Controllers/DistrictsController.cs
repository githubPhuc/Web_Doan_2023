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
    public class DistrictsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public DistrictsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/Districts
        [HttpGet]
        public async Task<IActionResult> GetDistrict(string ?name)
        {
            var data = await (from a in db_.District
                              join b in db_.City on a.IdCity equals b.Id
                                where (name == null || name == "" || a.NameDistrict.Contains(name))
                                select new
                                {
                                    Id = a.Id,
                                    NameDistrict = a.NameDistrict,
                                    NameCity = b.NameCity,
                                    Status = a.Status,
                                }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        // GET: api/Districts
        [HttpGet("LoadOnCity")]
        public async Task<IActionResult> Load_On_City(int idCity)
        {
            var data = await (from a in db_.District
                              where a.IdCity == idCity
                              select new
                              {
                                  Id = a.Id,
                                  NameDistrict = a.NameDistrict,
                                  Status = a.Status,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }

        // POST: api/Districts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<District>> PostDistrict(string NameDistrict, int idCity)
        {
            if (String.IsNullOrEmpty(NameDistrict))
            {
                return Ok(new Response { Status = "Failed", Message = "City name is null!" });
            }
            else
            {
                var check = await db_.District.Where(a => a.NameDistrict == NameDistrict&& a.IdCity ==idCity).ToListAsync();
                if (check.Count() > 0)
                {
                    return Ok(new Response { Status = "Failed", Message = "District name exists in database!" });
                }
                var data = new District()
                {
                    IdCity= idCity,
                    NameDistrict = NameDistrict,
                    Status = true,
                };
                db_.District.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "District created successfully!" });
            }
        }

        // DELETE: api/Districts/5
        [HttpPost("DeleteDistrict")]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            var checkWards = await db_.Wards.Where(a => a.IdDistrict == id).ToListAsync();
            if (checkWards.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "Wards in database!" });
            }
            else
            {
                var data = await db_.District.FirstOrDefaultAsync(a => a.Id == id);
                if (data != null)
                {
                    db_.District.Remove(data);
                    await db_.SaveChangesAsync();

                    return Ok(new Response { Status = "Success", Message = "District delete successfully!" });
                }
                return Ok(new Response { Status = "Failed", Message = "District delete failed!" });
            }

        }

        private bool DistrictExists(int id)
        {
            return (db_.District?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
