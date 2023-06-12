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
    public class CitiesController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public CitiesController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<IActionResult> GetCity(string ?name)
        {
            var data = await (from a in db_.City
                              where (
                                       (name == null || name == "" || a.NameCity.Contains(name))
                                    )
                              select new
                              {
                                  Id=a.Id,
                                  Name=a.NameCity,
                                  Status=a.Status,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(string CityName)
        {
            if(String.IsNullOrEmpty(CityName))
            {
                return Ok(new Response { Status = "Failed", Message = "City name is null!" });
            }
            else
            {
                var check =await db_.City.Where(a=>a.NameCity==CityName).ToListAsync();
                if(check.Count()>0)
                {
                    return Ok(new Response { Status = "Failed", Message = "City name exists in database!" });
                }
                var data = new City()
                {
                    NameCity = CityName,
                    Status = true,
                };
                db_.City.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "City created successfully!" });
            }
        }

        // DELETE: api/Cities/5
        [HttpPost("DeleteCity")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var checkDistrict= await db_.District.Where(a=>a.IdCity ==id).ToListAsync();
            if (checkDistrict.Count() > 0)
            {
                return Ok(new Response { Status = "Failed", Message = "District in database!" });
            }
            else
            {
                var data = await db_.City.FirstOrDefaultAsync(a => a.Id == id);
                if (data != null)
                {
                    db_.City.Remove(data);
                    await db_.SaveChangesAsync();

                    return Ok(new Response { Status = "Success", Message = "City delete successfully!" });
                }
                return Ok(new Response { Status = "Failed", Message = "City delete failed!" });
            }

        }

        private bool CityExists(int id)
        {
            return (db_.City?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
