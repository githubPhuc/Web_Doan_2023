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
    public class BillOfSalesController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public BillOfSalesController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/BillOfSales
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? code)
        {
            var list = (from a in db_.BillOfSale
                        where a.IsDelete == false
                        select new
                        {
                            Id = a.Id,
                            Name = a.Name,
                            code = a.code,
                            price = a.price,
                            createDate = a.createDate,
                            updateDate = a.updateDate,
                            deleteDate = a.deleteDate,
                            UsernameCreate = a.UsernameCreate,
                            UsernameDelete = a.UsernameDelete,
                            UsernameUpdate = a.UsernameUpdate,
                            IsDelete = a.IsDelete,
                            StatusBill = a.StatusBill,
                            StatusCode = a.StatusCode,
                        }).ToList();
            var data = list.Where(a=>(code==null||code==""||a.code.ToUpper().Contains(code.ToUpper()))).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }


        private bool BillOfSaleExists(int id)
        {
            return (db_.BillOfSale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
