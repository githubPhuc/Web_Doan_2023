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
    public class productDepotsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;
        public productDepotsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/productDepots
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? nameDepot, string? nameProduct)
        {
            var List = (from a in db_.productDepot
                        select new
                        {
                            Id = a.Id,
                            idDepot = a.idDepot,
                            nameDepot = (a.idDepot == 0|| a.idDepot==null)?"":db_.Depot.Where(c=>c.Id==a.idDepot).FirstOrDefault().nameDepot,
                            idProduct=  a.idProduct,
                            price=a.price,
                            nameProduct = (a.idProduct == 0 || a.idProduct == null) ? "" : db_.Product.Where(c => c.Id == a.idProduct).FirstOrDefault().nameProduct,
                            QuantityProduct = a.QuantityProduct,
                        }).ToList();
            var data = List.Where(a => (nameDepot == "" || nameDepot == null || a.nameDepot.ToUpper().Contains(nameDepot.ToUpper())) &&
            (nameProduct == "" || nameProduct == null || a.nameProduct.ToUpper().Contains(nameProduct.ToUpper()))).ToList();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpPost("SetPriceOnProduct")]
        public async Task<ActionResult> SetPriceOnProduct(int id)
        {
            var data = db_.productDepot.Where(a => a.Id == id).FirstOrDefault();
            if(data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Data is null!" });
            }
            else
            {
                var dataPro = db_.Product.Where(a => a.Id == data.idProduct).FirstOrDefault();
                dataPro.price = data.price;
                dataPro.IdDepot = data.idDepot;
                db_.Entry(dataPro).State = EntityState.Modified;
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Update price successfully!" });
            }
            
        }

        private bool productDepotExists(int id)
        {
            return (db_.productDepot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
