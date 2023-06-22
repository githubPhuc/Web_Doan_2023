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
        public async Task<ActionResult> _GetList(string? code,string? dateStar,string? DateEnd)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
            var _NgayBatDau = new DateTime();
            var _NgayKetthuc = new DateTime();
            DateTime _NgayBD = new DateTime();
            DateTime _NgayKT = new DateTime();
            if (!string.IsNullOrEmpty(dateStar))
            {
                try
                {
                    _NgayBatDau = DateTime.ParseExact(dateStar, "dd/MM/yyyy", cul);

                    _NgayBD = new DateTime(_NgayBatDau.Year, _NgayBatDau.Month, _NgayBatDau.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(DateEnd))
            {
                try
                {
                    _NgayKetthuc = DateTime.ParseExact(DateEnd, "dd/MM/yyyy", cul);

                    _NgayKT = new DateTime(_NgayKetthuc.Year, _NgayKetthuc.Month, _NgayKetthuc.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            var data = await db_.enterWarehouseVouchers.Where(
                a=>(code == null|| code == ""||a.codeEnterWarehouseVouchers.Contains(code))
               
                ).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }


    }
}
