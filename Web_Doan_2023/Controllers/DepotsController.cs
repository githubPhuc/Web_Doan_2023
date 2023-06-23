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
    public class DepotsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public DepotsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/Depots
        [HttpGet("Get")]
        public async Task<ActionResult> GetDepot(string? name, string? code)
        {

            var data = await (from a in db_.Depot
                              where (
                                       (name == null || name == "" || a.nameDepot.Contains(name)) &&
                                       (code == null || code == "" || a.codeDepot.Contains(code))
                                    )
                              select new
                              {
                                  Id = a.Id,
                                  codeDepot = a.codeDepot,
                                  nameDepot = a.nameDepot,
                                  Location = a.Location,
                                  Phone = a.Phone,
                                  storekeepers = a.storekeepers,
                                  status = a.status,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpGet("FindID")]
        public async Task<ActionResult> FindID(int id)
        {

            var data = await (from a in db_.Depot
                              where a.Id==id
                              select new
                              {
                                  Id = a.Id,
                                  codeDepot = a.codeDepot,
                                  nameDepot = a.nameDepot,
                                  Location = a.Location,
                                  Phone = a.Phone,
                                  storekeepers = a.storekeepers,
                                  status = a.status,
                              }).FirstOrDefaultAsync();
            return Ok(new
            {
                acc = data,
            });
        }

        // PUT: api/Depots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Put")]
        public async Task<IActionResult> PutDepot(int id, Depot depot)// Lỗi rồi
        {
            
            string code = depot.codeDepot.ToUpper();
            if (code.Length != 3)
            {
                return Ok(new Response { Status = "Failed", Message = "Code depots in three characters long" });
            }
            var dataDepots = db_.Depot.Where(x => x.Id == id).FirstOrDefault();
            if(dataDepots == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Update Depots failed!" });
            }
            else
            {
                dataDepots.codeDepot = code;
                dataDepots.nameDepot = depot.nameDepot;
                dataDepots.Phone = depot.Phone;
                dataDepots.Location = depot.Location;
                dataDepots.status = depot.status;
                dataDepots.storekeepers = depot.storekeepers;

                db_.Entry(dataDepots).State = EntityState.Modified;
                try
                {
                    await db_.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepotExists(id))
                    {
                        return Ok(new Response { Status = "Failed", Message = "Update Depots failed!" });
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new Response { Status = "Success", Message = "Depot update successfully!" });
            }
        }

        // POST: api/Depots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Depot>> PostDepot(Depot depot)
        {
            if (db_.Depot == null)
            {
                return Problem("Entity set 'Web_Doan_2023Context.Depot'  is null.");
            }
            string code = depot.codeDepot.ToUpper();
            if (code.Length != 3)
            {
                return Ok(new Response { Status = "Failed", Message = "Code depots in three characters long" });

            }
            var dataDepots = new Depot()
            {
                codeDepot = depot.codeDepot.ToUpper(),
                nameDepot = depot.nameDepot,
                Phone = depot.Phone,
                Location = depot.Location,
                status = true,
                storekeepers = depot.storekeepers,
            };
            db_.Depot.Add(dataDepots);
            await db_.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Depot create successfully!" });
        }

        // DELETE: api/Depots/5
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteDepot(int id)
        {

            var data = await db_.Depot.FirstOrDefaultAsync(a => a.Id == id);
            if (data != null)
            {
                db_.Depot.Remove(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Depot delete successfully!" });
            }
            return Ok(new Response { Status = "Failed", Message = "Depot delete failed!" });

        }

        private bool DepotExists(int id)
        {
            return (db_.Depot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
