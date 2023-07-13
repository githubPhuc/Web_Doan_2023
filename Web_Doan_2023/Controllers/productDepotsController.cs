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
        [HttpGet("GetViewListProductUser")]
        public async Task<ActionResult> GetViewListProductUser(string? name)
        {
            var list = (from a in db_.productDepot
                        join b in db_.Product on a.idProduct equals b.Id
                        where a.status == true
                        select new
                        {
                            ShipmentCode = a.ShipmentCode,
                            Id = b.Id,
                            codeproduct = b.codeProduct,
                            nameproduct = b.nameProduct,
                            Descriptionproduct = b.Description,
                            idCategory = b.idCategory,
                            nameCategory = (b.idCategory == 0 || b.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == b.idCategory).FirstOrDefault().nameCategory,
                            idProducer = b.idProducer,
                            nameProduce = (b.idProducer == 0 || b.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == b.idProducer).FirstOrDefault().nameProduce,
                            price = a.priceSale,
                            RamProduct = b.RamProduct,
                            RamName = (b.RamProduct == 0 || b.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == b.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = b.SSDProduct,
                            SSDName = (b.SSDProduct == 0 || b.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == b.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = b.CPUProduct,
                            CPUName = (b.CPUProduct == 0 || b.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == b.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = b.DisplayProduct,
                            DisplayName = (b.DisplayProduct == 0 || b.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == b.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = b.ColorProduct,
                            ColorName = (b.ColorProduct == 0 || b.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == b.ColorProduct).FirstOrDefault().Name,
                            portConnection = b.portConnection,
                            CardDisplay = b.CardDisplay,
                            CardDisplayName = (b.CardDisplay == 0 || b.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == b.CardDisplay).FirstOrDefault().Name,
                            mainboar = b.MainProduct,
                            AccessoriesIncluded = b.AccessoriesIncluded,
                            Status = b.Status,
                            idSale = b.idSale,
                            SaleName = (b.idSale == 0 || b.idSale == null) ? "" : db_.Sale.Where(c => c.Id == b.idSale).FirstOrDefault().nameSale,
                            IsDelete = b.IsDelete,
                            QuantityProduct = a.QuantityProduct,
                        }).ToList();
            var data = list.Where(a => (
                               (name == "" || name == null || 
                                a.nameproduct.ToUpper().Contains(name.ToUpper()) ||
                                a.nameProduce.ToUpper().Contains(name.ToUpper()) ||
                                a.RamName.ToUpper().Contains(name.ToUpper()) ||
                                a.CPUName.ToUpper().Contains(name.ToUpper()) ||
                                a.DisplayName.ToUpper().Contains(name.ToUpper()) ||
                                a.ColorName.ToUpper().Contains(name.ToUpper()) ||
                                a.CardDisplayName.ToUpper().Contains(name.ToUpper())))).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });

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
                            priceSale = a.priceSale,
                            ShipmentCode = a.ShipmentCode,
                            nameProduct = (a.idProduct == 0 || a.idProduct == null) ? "" : db_.Product.Where(c => c.Id == a.idProduct).FirstOrDefault().nameProduct,
                            QuantityProduct = a.QuantityProduct,
                            status = a.status,
                            DateCreate = a.DateCreate
                        }).ToList();
            var data = List.Where(a => (nameDepot == "" || nameDepot == null || a.nameDepot.ToUpper().Contains(nameDepot.ToUpper())) &&
            (nameProduct == "" || nameProduct == null || a.nameProduct.ToUpper().Contains(nameProduct.ToUpper()))).OrderByDescending(a=>a.DateCreate).ToList();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpPost("SetPriceSale")]
        public async Task<ActionResult> SetPriceSale(int id,decimal priceSale)
        {
            var data = db_.productDepot.Where(a => a.Id == id).FirstOrDefault();
            if(data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Data is null!" });
            }
            else
            {
                data.priceSale = priceSale;
                db_.Entry(data).State = EntityState.Modified;
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Update price successfully!" });
            }
            
        }
        [HttpPost("SetStatus")]
        public async Task<ActionResult> SetStatus(int id,int idProduct)
        {
            var list = db_.productDepot.Where(a => a.idProduct == idProduct).ToList();
            if (list.Count()==0)
            {
                return Ok(new Response { Status = "Failed", Message = "Data is null!" });
            }
            else
            {
                foreach (var item in list)
                {
                    item.status = false;
                    db_.Entry(item).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                }
                var data = list.Where(a => a.Id == id).FirstOrDefault();
                if (data.priceSale>0)
                {
                    data.status = true;
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Update price successfully!" });
                }
                else
                {
                    return Ok(new Response { Status = "Failed", Message = "Price has not been updated! Please check again" });
                }
            }
        }
        [HttpGet("inventoryDepot")]
        public async Task<ActionResult> inventoryDepot()
        {

            return Ok(new Response { Status = "Failed", Message = "Data is null!" });
        }

        private bool productDepotExists(int id)
        {
            return (db_.productDepot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
