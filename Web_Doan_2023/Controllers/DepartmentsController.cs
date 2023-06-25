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
    public class DepartmentsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public DepartmentsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }
        [HttpGet("GetList")]
        public async Task<IActionResult> Get(string? name)
        {
            var data = await (from a in db_.Department
                       where a.Status ==true && (name== null||name==""|| a.nameDepartment.ToUpper().Contains(name.ToUpper()))
                       select a).ToListAsync();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListComboBox")]
        public async Task<IActionResult> Get()
        {
            var data = await (from a in db_.Department
                              where a.Status == true
                              select a).ToListAsync();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpPost("Insert")]
        public async Task<ActionResult> Post(string codeDepartment, string nameDepartment)
        {
            var check = await db_.Department.Where(a=>a.codeDepartment== codeDepartment).FirstOrDefaultAsync();
            if (check != null)
            {
                return Ok(new Response { Status = "Failed", Message = "Code Department already exist!" });
            }
            else
            {
                try
                {
                    var data = new Department()
                    {
                        codeDepartment = codeDepartment,
                        nameDepartment = nameDepartment,
                        Status = true
                    };
                    db_.Department.Add(data);
                    db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Insert Department " + nameDepartment + " successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var data = await db_.Department.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Department not  exist!" });
            }
            else
            {
                try
                {
                    var checkUser = db_.Users.Where(a=>a.idDepartment ==id).ToList();
                    if (checkUser.Count() > 0)
                    {
                        return Ok(new Response { Status = "Failed", Message = "The users exists in department!" });
                    }
                    db_.Remove(data);
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Delete Department " + data.nameDepartment + " successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
        }

        private bool DepartmentExists(int id)
        {
            return (db_.Department?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
