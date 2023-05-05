using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;
        private readonly IWebHostEnvironment _environment;
        public ProductsController(Web_Doan_2023Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult> GetProduct()
        {
            var result = (from a in _context.Product
                          join b in _context.Producer on a.idProducer equals b.Id
                          select new
                          {
                              Id = a.Id,
                              codeproduct = a.codeProduct,
                              nameproduct = a.nameProduct,
                              categoryproduct = a.nameCategory,
                              Descriptionproduct = a.Description,
                              produceproduct = b.nameProduce,
                          }).ToArray();
            return Ok(new
            {
                pro = result,
                count = result.Count()
            });
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
          if (_context.Product == null)
          {
                return Ok(new Response { Status = "Failed", Message = "Not Data!" });
            }
            var data = (from a in _context.Product
                       
                       where a.Id == id
                       select new
                       {
                           mainproduct = a.MainProduct,
                           ramProduct = a.RamProduct,
                           cpuProduct = a.CPUProduct,
                           ssdProduct =a.SSDProduct,
                           displayProduct = a.DisplayProduct,
                           colorProduct     = a.ColorProduct,
                           price= a.price,

                       }).ToList();


            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Not Data!" });
            }

            return Ok(new
            {
                pro = data,
                count = data.Count()
            });
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> createProduct(Product product, List<IFormFile> uploadFile)
        {
            if (_context.Product == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Entity set 'Web_Doan_2023Context.Product'  is null.!" });
            }
            if (ModelState.IsValid)
            {

                var pro = new Product()
                {
                    codeProduct = product.codeProduct,
                    nameProduct = product.nameProduct,
                    Description = product.Description,
                    price = product.price,
                    CPUProduct = product.CPUProduct,
                    RamProduct = product.RamProduct,
                    DisplayProduct = product.DisplayProduct,
                    Status = true,
                    idCategory = product.idCategory,
                    MainProduct = product.MainProduct,
                    ColorProduct = product.ColorProduct,
                    IsDelete = false,
                    AccessoriesIncluded = product.AccessoriesIncluded,
                    idSale = product.idSale,
                };
                _context.Product.Add(pro);
                foreach (var file in uploadFile)
                {
                    string fileName = pro.codeProduct;
                    string filePath = GetFilePath(fileName);
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    string imagePath = filePath + "\\image" + pro.Id + ".png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);

                    }
                }


                
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    id = pro.Id,
                });
            }
            return Ok(new Response { Status = "Failed", Message = "Create product failed!" });

        }
        [HttpPost]
        [Route("uploadImage")]
        public async Task<ActionResult> uploadImage(int id, List<IFormFile> uploadFile)
        {
            bool results = false;
            try
            {
                var dataProduct = _context.Product.FirstOrDefault(a => a.Id == id);
                if(dataProduct==null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Product is null!" });

                }
                // var uploadFile = Request.Form.Files;
                foreach (var file in uploadFile)
                {
                    string fileName = dataProduct.codeProduct;
                    string filePath = GetFilePath(fileName);
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    string imagePath = filePath + "\\image"+id+".png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        results = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            return Ok(results);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [NonAction]
        private string GetFilePath(string productCode)
        {
            return this._environment.WebRootPath+"\\image\\product\\"+ productCode;
        }
    }
}
