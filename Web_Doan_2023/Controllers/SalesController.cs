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
    public class SalesController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public SalesController(Web_Doan_2023Context context)
        {
            db_ = context;
        }
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList()
        {
            var data = db_.Sale.ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        
    }
}
