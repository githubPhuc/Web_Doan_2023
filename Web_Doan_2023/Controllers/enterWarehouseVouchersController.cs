using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class enterWarehouseVouchersController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public enterWarehouseVouchersController(Web_Doan_2023Context context)
        {
            db_ = context;
        }
        [HttpGet("GetList")]
        public async Task<ActionResult> _GetList(string? code)
        {

            var data = await db_.enterWarehouseVouchers.Where(
                a => (code == null || code == "" || a.codeEnterWarehouseVouchers.Contains(code))

                ).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }


    }
}


