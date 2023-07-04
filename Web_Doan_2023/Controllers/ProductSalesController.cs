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
    public class ProductSalesController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public ProductSalesController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/ProductSales
        [HttpGet("GetListSale")]
        public async Task<ActionResult> GetListSale()
        {
            var data = await db_.Sale.ToListAsync();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListSaleOnIdProduct")]
        public async Task<ActionResult> GetListSaleOnIdProduct(int id)
        {
            var data = (from a in db_.Sale
                        join b in db_.ProductSale on a.Id equals b.saleId
                        where b.productId == id
                        select new
                        {
                            id=a.Id,
                            productId=b.productId,
                            saleId = b.saleId,
                            nameSale = a.nameSale,
                            descriptionSale = a.descriptionSale,
                            marth = a.marth,
                            Unit = a.Unit,
                            Status = a.Status,
                        }).ToList();

            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListSaleByID")]
        public async Task<ActionResult> GetListSaleByID(int id)
        {
            var data = (from a in db_.Sale
                        where a.Id == id
                        select new
                        {
                            id = a.Id,
                            nameSale = a.nameSale,
                            descriptionSale = a.descriptionSale,
                            marth = a.marth,
                            Unit = a.Unit,
                            Status = a.Status,
                        }).FirstOrDefault();

            return Ok(new
            {
                data = data
            });
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Sale model,int id)
        {
            var check = db_.Sale.Where(a => a.nameSale == model.nameSale && a.marth == model.marth).FirstOrDefault();
            if (check == null)
            {
                try
                {
                    var data = db_.Sale.Where(a=>a.Id==id).FirstOrDefault();
                    data.nameSale=model.nameSale;
                    data.marth=model.marth;
                    data.descriptionSale=model.descriptionSale;
                    data.Unit=model.Unit;
                    data.Status=model.Status;
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                   
                    return Ok(new Response { Status = "Success", Message = "Update sale successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "Sale name is exist" });
            }

        }
        [HttpPost("UpdateProductSale")]
        public async Task<ActionResult> UpdateProductSale(int idProduct,int IdSale)
        {
            var check = db_.ProductSale.Where(a => a.productId == idProduct && a.saleId == IdSale).FirstOrDefault();
            if (check == null)
            {
                try
                {
                    check.saleId = IdSale;
                    check.productId=idProduct;
                    db_.Entry(check).State = EntityState.Modified;
                    await db_.SaveChangesAsync();

                    return Ok(new Response { Status = "Success", Message = "Update product sale successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "product sale is exist" });
            }

        }
        [HttpPost("Insert")]
        public async Task<ActionResult> Insert(Sale model,int idproduct)
        {
            var check = db_.Sale.Where(a=>a.nameSale==model.nameSale&& a.marth==model.marth).FirstOrDefault();
            if(check == null)
            {
                try
                {
                    var data = new Sale()
                    {
                        nameSale = model.nameSale,
                        descriptionSale = model.descriptionSale,
                        marth = model.marth,
                        Unit = model.Unit,
                        Status = model.Status,
                    };
                    db_.Sale.Add(data);
                    await db_.SaveChangesAsync();
                    if(idproduct!=null&&idproduct!=0)
                    {
                        var dataProSale = new ProductSale()
                        {
                            productId = idproduct,
                            saleId = data.Id,
                        };
                        db_.ProductSale.Add(dataProSale);
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Insert product sale successfully!" });
                    }
                    return Ok(new Response { Status = "Success", Message = "Insert sale successfully!" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "Sale name is exist" });
            }
            
        }

        [HttpPost("DeleteSale")]
        public async Task<ActionResult> DeleteSale(int idSale)
        {
            var check  = await db_.ProductSale.Where(a=>a.saleId == idSale).ToListAsync();
            if (check.Count() > 0) {
                return Ok(new Response { Status = "Failed", Message = "id product is exist" });
            }
            else
            {
                try
                {
                    var data = db_.Sale.Where(a=>a.Id==idSale).FirstOrDefault();
                    if (data != null)
                    {
                        db_.Remove(data);
                        db_.SaveChanges();
                        return Ok(new Response { Status = "Success", Message = "Delete is successfully" });
                    }
                    return Ok(new Response { Status = "Failed", Message = "Delete is Failed" });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }

        }

        private bool ProductSaleExists(int id)
        {
            return (db_.ProductSale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
