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
    public class CartProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public CartProductsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/CartProducts
        [HttpGet("GetCartProduct")]
        public async Task<ActionResult> GetCartProduct(string Username)
        {

            var result = (from a in db_.CartProduct
                          join b in db_.Product on a.ProductId equals b.Id
                          where a.Username == Username
                          select new
                          {
                              Id = a.Id,
                              Username = a.Username,
                              nameProduct = b.nameProduct,
                              ProductId = a.ProductId,
                              Quantity = a.Quantity,
                              price = a.Price * a.Quantity,
                              salePrice = a.salePrice,

                          }).ToList();

            var total = result.Sum(s => s.price);
            return Ok(new
            {
                cart = result,
                total = total,
            });

        }
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {

            var result = (from a in db_.CartProduct
                          join b in db_.Product on a.ProductId equals b.Id
                          select new
                          {
                              Id = a.Id,
                              Username = a.Username,
                              nameProduct = b.nameProduct,
                              ProductId = a.ProductId,
                              Quantity = a.Quantity,
                              price = a.Price*a.Quantity,
                              salePrice = a.salePrice,

                          }).ToList();

            var total = result.Sum(s => s.price);
            return Ok(new
            {
                cart = result,
                total = total,
            });

        }


        // POST: api/CartProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addCart")]
        public async Task<ActionResult<CartProduct>> addCart(int idProduct, int Quantity, string User, int idSale)
        {
            var check = await db_.CartProduct.Where(c => c.ProductId == idProduct && c.Username == User).FirstOrDefaultAsync();

            var pro = db_.productDepot.FirstOrDefault(a => a.idProduct == idProduct);
            if (pro != null)
            {
                if (Quantity > pro.QuantityProduct)
                {
                    return Ok(new Response { Status = "Failed", Message = "Insufficient inventory quantity!" });
                }
                else
                {

                    if (check != null)
                    {

                        if (idSale != 0)
                        {
                            var dataProduct = db_.Product.FirstOrDefault(b => b.Id == check.ProductId);
                            var dataSale = (from b in db_.Sale
                                            where b.Id == idSale
                                            select new
                                            {
                                                b.Id,
                                                b.nameSale,
                                                b.marth,
                                                b.Unit,
                                                b.Status,
                                            }).FirstOrDefault();
                            decimal PriceSale = 0;
                            if (dataSale != null)
                            {
                                if (dataSale.Unit == "VND")
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price) - Convert.ToDecimal(dataSale.marth);
                                }
                                else if (dataSale.Unit == "%")
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price) - (Convert.ToDecimal(dataProduct.price) * Convert.ToDecimal(dataSale.marth));
                                }
                                else
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price);
                                }
                            }
                            check.salePrice = PriceSale;
                        }
                        check.Quantity += Quantity;
                        db_.Entry(check).State = EntityState.Modified;
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Update cart successfully!" });
                    }
                    else
                    {
                        var dataSale = (from b in db_.Sale
                                        where b.Id == idSale
                                        select new
                                        {
                                            b.Id,
                                            b.nameSale,
                                            b.marth,
                                            b.Unit,
                                            b.Status,
                                        }).FirstOrDefault();
                        var dataProduct = db_.Product.FirstOrDefault(b => b.Id == idProduct);
                        if (dataProduct == null)
                        {
                            return Ok(new Response { Status = "Failed", Message = "Product does not exist!" });
                        }
                        else
                        {

                            decimal PriceSale = 0;
                            if (dataSale != null)
                            {
                                if (dataSale.Unit == "VND")
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price) - Convert.ToDecimal(dataSale.marth);
                                }
                                else if (dataSale.Unit == "%")
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price) - (Convert.ToDecimal(dataProduct.price) * Convert.ToDecimal(dataSale.marth));
                                }
                                else
                                {
                                    PriceSale = Convert.ToDecimal(dataProduct.price);
                                }
                            }
                            var cart = new CartProduct();
                            cart.Username = User;
                            cart.ProductId = idProduct;
                            cart.Price = (decimal)dataProduct.price!;
                            cart.Quantity = Quantity;
                            cart.salePrice = PriceSale;
                            cart.ProductName = dataProduct.nameProduct;
                            cart.CreatedDate = DateTime.Now;
                            cart.Status = true;
                            db_.Add(cart);
                            await db_.SaveChangesAsync();
                            return Ok(new Response { Status = "Success", Message = "Insert cart successfully!" });
                        }
                    }
                }
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "No products found in stock!" });
            }

        }

        //DELETE: api/CartProducts/5
        [HttpPost]
        [Route("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = db_.CartProduct.FirstOrDefault(a => a.Id == id);
            if (cart != null)
            {
                db_.CartProduct.Remove(cart);
                await db_.SaveChangesAsync();
                return Ok(new
                {
                    Status = 200,
                    msg = "Đã xoá khỏi giỏ hàng"
                });
            }
            return Ok(new
            {
                status = 500,
                msg = "Delete cart fail!!"
            });
        }
        [HttpPost]
        [Route("RemoveAllCart")]
        public async Task<IActionResult> RemoveAllCart(string userID)
        {
            var cart = await db_.CartProduct.Where(c => c.Username == userID).ToListAsync();
            if (cart != null)
            {
                foreach (var c in cart)
                {
                    db_.Remove(c);
                }
                await db_.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    msg = "Đã xoá toàn bộ giỏ hàng "
                });
            }
            return Ok(new
            {
                status = 500,
                msg = "Delete cart fail!!"
            });
        }

        private bool CartProductExists(int id)
        {
            return (db_.CartProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
