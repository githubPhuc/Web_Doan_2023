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
        private readonly Web_Doan_2023Context _context;

        public CartProductsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/CartProducts
        [HttpGet]
        public async Task<IActionResult> GetCartProduct(string UserID)
        {
            if (_context.CartProduct == null)
            {
                return NotFound();
            }
            var result = (from a in _context.CartProduct
                          join b in _context.Product on a.ProductId equals b.Id
                          where a.userID == UserID
                          select new
                          {
                              Id = a.Id,
                              nameProduct = b.nameProduct,
                              ProductId = a.ProductId,
                              Quantity = a.Quantity,
                              price = (a.salePrice == 0 ? b.price : (b.price - a.salePrice)) * a.Quantity,
                              salePrice = a.salePrice,
                              image = _context.Images.Where(d => d.idProduct == a.Id).Select(d => d.nameImage).ToList().Take(1),

                              SoLuongCart = (from a in _context.CartProduct
                                             where a.userID == UserID
                                             select a).Count()
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
            var cart = await _context.CartProduct.FindAsync(cartID);
            var proid = cart.ProductId;
            var pro = await _context.productDepot.FindAsync(proid);
            cart.Quantity += Quantity;
            cart.salePrice = (sale!=0?sale: cart.salePrice);
            if (cart.Quantity == 0 || cart.Quantity > 5)
            {
                return BadRequest();
            }
            if (cart.Quantity > pro.QuantityProduct)
            {
                return Ok(new
                {
                    Status = 500,
                    msg = "Số lượng sản phẩm không đủ"
                });
            }
            if (cart.Quantity <= 0)
            {
                cart.Quantity = 0;
            }
            _context.Update(cart);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = 200
            });
        }

        // POST: api/CartProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addCart")]
        public async Task<ActionResult<CartProduct>> addCart(int idProduct, int Quantity, string userID, decimal sale)
        {
            var check = await _context.CartProduct.Where(c => c.ProductId == idProduct && c.userID == userID).FirstOrDefaultAsync();
            var pro = await _context.productDepot.FindAsync(idProduct);

            if (Quantity > pro.QuantityProduct)
            {
                return Ok(new
                {
                    status = 500,
                    msg = "Số lượng sản phẩm không đủ"
                });
            }

            if (check != null)
            {
                check.Quantity += Quantity;
                _context.Update(check);
                await _context.SaveChangesAsync();
            }
            else
            {
                var dataProduct = _context.Product.FirstOrDefault(b => b.Id == idProduct);
                var cart = new CartProduct();
                cart.userID = userID;
                cart.ProductId = idProduct;
                cart.Price = (decimal)dataProduct.price;
                cart.Quantity = Quantity;
                cart.salePrice = sale;
                cart.ProductName = dataProduct.nameProduct;
                cart.CreatedDate = DateTime.Now;
                cart.Status = false;
                _context.Add(cart);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                status = 200,
                msg = "Đã thêm vào giỏ hàng"
            });
        }

        //DELETE: api/CartProducts/5
        [HttpPost]
        [Route("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = _context.CartProduct.FirstOrDefault(a => a.Id == id);
            if (cart != null)
            {
                _context.CartProduct.Remove(cart);
                await _context.SaveChangesAsync();
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
            var cart = await _context.CartProduct.Where(c => c.userID == userID).ToListAsync();
            if (cart != null)
            {
                foreach (var c in cart)
                {
                    _context.Remove(c);
                }
                await _context.SaveChangesAsync();
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
            return (_context.CartProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
