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
        public async Task<ActionResult<IEnumerable<CartProduct>>> GetCartProduct(string UserID)
        {
            if (_context.CartProduct == null)
            {
                return NotFound();
            }
            var result = (from a in _context.CartProduct
                          join c in _context.CartDetailProduct on a.Id equals c.IdCartProduct
                          join b in _context.Product on c.ProductId equals b.Id
                          
                          where a.userCreate == UserID
                          select new
                          {
                              Id = a.Id,
                              TenSanPham = b.nameProduct,
                              IdSanPham = c.IdCartProduct,
                              SoLuong = c.Quantity,
                              GiaSanPham = b.price,
                              //hình ảnh , giá cả , giảm giá
                              SoLuongCart = (from c in _context.CartDetailProduct
                                             join a in _context.CartProduct on a.Id equals c.IdCartProduct
                                             where  a.userCreate == UserID
                                             select c).Count()
                          }).ToList();

            var total = result.Sum(s => s.GiaSanPham);
            return Ok(new
            {
                cart = result,
                total = total,
            });

        }

        // GET: api/CartProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartProduct>> GetCartProduct(int id)
        {
          if (_context.CartProduct == null)
          {
              return NotFound();
          }
            var cartProduct = await _context.CartProduct.FindAsync(id);

            if (cartProduct == null)
            {
                return NotFound();
            }

            return cartProduct;
        }

        // PUT: api/CartProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartProduct(int id, CartProduct cartProduct)
        {
            if (id != cartProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CartProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartProduct>> PostCartProduct(CartProduct cartProduct)
        {
          if (_context.CartProduct == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.CartProduct'  is null.");
          }
            _context.CartProduct.Add(cartProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartProduct", new { id = cartProduct.Id }, cartProduct);
        }

        // DELETE: api/CartProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartProduct(int id)
        {
            if (_context.CartProduct == null)
            {
                return NotFound();
            }
            var cartProduct = await _context.CartProduct.FindAsync(id);
            if (cartProduct == null)
            {
                return NotFound();
            }

            _context.CartProduct.Remove(cartProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartProductExists(int id)
        {
            return (_context.CartProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
