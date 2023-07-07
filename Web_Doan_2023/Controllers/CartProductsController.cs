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
        public async Task<IActionResult> GetCartProduct(string Username)
        {
            
            var result = (from a in db_.CartProduct
                          join b in db_.Product on a.ProductId equals b.Id
                          where a.Username == Username
                          select new
                          {
                              Id = a.Id,
                              nameProduct = b.nameProduct,
                              ProductId = a.ProductId,
                              Quantity = a.Quantity,
                              price = (a.salePrice == 0 ? b.price : (b.price - a.salePrice)) * a.Quantity,
                              salePrice = a.salePrice,
                              image = db_.Images.Where(d => d.idProduct == a.Id).Select(d => d.PathImage).ToList().Take(1),
                              SoLuongCart = (from a in db_.CartProduct
                                             where a.Username == Username
                                             select a).Count()
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
                              nameProduct = b.nameProduct,
                              ProductId = a.ProductId,
                              ProductName = (a.ProductId==0 )?"":db_.Product.FirstOrDefault(c=>c.Id==a.ProductId).nameProduct,
                              Quantity = a.Quantity,
                              price = (a.salePrice == 0 ? b.price : (b.price - a.salePrice)) * a.Quantity,
                              salePrice = a.salePrice,
                            
                          }).ToList();

            var total = result.Sum(s => s.price);
            return Ok(new
            {
                cart = result,
                total = total,
            });

        }

        // PUT: api/CartProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Update")]
        public async Task<IActionResult> Update(int cartID, int Quantity, decimal sale = 0)
        {
            var cart = await db_.CartProduct.FindAsync(cartID);
            var proid = cart.ProductId;
            var pro = await db_.productDepot.FindAsync(proid);
            cart.Quantity += Quantity;
            cart.salePrice = (sale!=0?sale: cart.salePrice);
            if (cart.Quantity == 0 || cart.Quantity > 5)
            {
                return BadRequest();
            }
            if (cart.Quantity > pro.QuantityProduct)
            {
                
                return Ok(new Response { Status = "Failed", Message = "Số lượng sản phẩm không đủ!" });
            }
            if (cart.Quantity <= 0)
            {
                cart.Quantity = 0;
            }
            db_.Update(cart);
            await db_.SaveChangesAsync();

            return Ok(new
            {
                status = 200
            });
        }

        // POST: api/CartProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addCart")]
        public async Task<ActionResult<CartProduct>> addCart(int idProduct, int Quantity, string User, int idSale)
        {
            var check = await db_.CartProduct.Where(c => c.ProductId == idProduct && c.Username == User).FirstOrDefaultAsync();
            var pro = await db_.productDepot.FindAsync(idProduct);

            if (Quantity > pro.QuantityProduct)
            {
                return Ok(new Response { Status = "Success", Message = "Add cart successfully!" });
            }

            if (check != null)
            {
                check.Quantity += Quantity;
                db_.Update(check);
                await db_.SaveChangesAsync();
            }
            else
            {
                var dataSale = (from a in db_.ProductSale
                                join b in db_.Sale on a.saleId equals b.Id
                                where a.productId == idProduct
                                where a.saleId== idSale
                               select new
                               {
                                   a.Id,
                                   b.nameSale,
                                   b.marth,
                                   b.Unit,
                                   b.Status,
                               }).FirstOrDefault();
                var dataProduct = db_.Product.FirstOrDefault(b => b.Id == idProduct);
                if (dataProduct != null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Sản phẩm không tồn tại!" });
                }
                var cart = new CartProduct();
                cart.Username = User;
                cart.ProductId = idProduct;
                cart.Price = (decimal)dataProduct.price;
                cart.Quantity = Quantity;
                cart.salePrice = (dataSale == null || dataSale.marth == 0) ? 0 : dataSale.marth;
                cart.ProductName = dataProduct.nameProduct;
                cart.CreatedDate = DateTime.Now;
                cart.Status = true;
                db_.Add(cart);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Insert cart successfully!" });
            }

            return Ok(new Response { Status = "Failed", Message = "Sản phẩm không tồn tại!" });
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
