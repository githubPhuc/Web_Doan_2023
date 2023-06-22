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
                          where a.IsDelete == false
                          select new
                          {
                              Id = a.Id,
                              codeproduct = a.codeProduct,
                              nameproduct = a.nameProduct,
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
        [HttpGet]
        [Route("GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
          if (_context.Product == null)
          {
                return Ok(new Response { Status = "Failed", Message = "Not Data!" });
            }
            var data = (from a in _context.Product
                       where a.Id == id 
                       where a.IsDelete == false
                       select new
                       {
                           Id = a.Id,
                           codeproduct = a.codeProduct,
                           nameproduct = a.nameProduct,
                           Descriptionproduct = a.Description,
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
        [HttpGet]
        [Route("GetProductOnCategory")]
        public async Task<ActionResult<Product>> GetProductOnCategory(int id)
        {
            if (_context.Product == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Not Data!" });
            }
            var data = (from a in _context.Product
                        where a.idCategory == id && a.IsDelete == false
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            mainproduct = a.MainProduct,
                            ramProduct = a.RamProduct,
                            cpuProduct = a.CPUProduct,
                            ssdProduct = a.SSDProduct,
                            displayProduct = a.DisplayProduct,
                            colorProduct = a.ColorProduct,
                            price = a.price,

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

       

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> createProduct(Product product)
        {
            
            if (ModelState.IsValid)
            {
                var codeColor = _context.ColorProduct.Where(a => a.Id == product.Id).FirstOrDefault();
                var pro = new Product()
                {
                    codeProduct = product.codeProduct.ToUpper() + codeColor.code.ToUpper(),
                    nameProduct = product.nameProduct,
                    Description = product.Description,
                    price = product.price,
                    CPUProduct = product.CPUProduct,
                    RamProduct = product.RamProduct,
                    DisplayProduct = product.DisplayProduct,
                    Status = true,
                    idCategory = product.idCategory,
                    SSDProduct = product.SSDProduct,
                    idProducer = product.idProducer,
                    MainProduct = product.MainProduct,
                    ColorProduct = product.ColorProduct,
                    IsDelete = false,
                    AccessoriesIncluded = product.AccessoriesIncluded,
                    idSale = product.idSale,
                };
                _context.Product.Add(pro);
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
                if (dataProduct == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Product is null!" });
                }//Kiễm tra sự tồn tại của sản phẩm 
                foreach (var file in uploadFile)
                {
                    var dataImage = new Images();//khởi tạo data image
                    string fileName = dataProduct.codeProduct; // lấy code product
                    if (fileName.Length != 9)
                    {
                        results = false;
                        return Ok(results);
                    }
                    string filePath = GetFilePath(fileName);
                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    string imagePath = filePath + "\\image" + id + ".png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    dataImage.nameImage = imagePath;
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        results = true;
                    }
                    dataImage.idProduct = id;
                    dataImage.nameImage = imagePath;
                    _context.Images.Add(dataImage);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            return Ok(results);
        }
        [HttpPost]
        [Route("updateImage")]
        public async Task<ActionResult> updateImage(int idImage, int idProduct, IFormFile updateFile)
        {
            bool results = false;
            try
            {
                var dataProduct = _context.Product.FirstOrDefault(a => a.Id == idProduct);
                if (dataProduct == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Product is null!" });
                }//Kiễm tra sự tồn tại của sản phẩm .
                var dataImageValues = _context.Images.FirstOrDefault(a => a.idProduct == idProduct && a.Id == idImage);
                if (dataImageValues == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Image in Product is not null!" });
                }//Kiểm tra ảnh trong image có tồn tại....
                string filePath = GetFilePath(dataProduct.codeProduct.Substring(0, 9));// Lấy mã sản phẩm từ 9 kitu đầu gồm 3 kí tự hoá đơn nhập kho+3 kí tự nhà cung cấp+3 í tự mã màu
                if (System.IO.Directory.Exists(filePath))
                {
                    var dataImage = new Images();//khởi tạo data image
                    dataImage.idProduct = idProduct;
                    string imagePath = filePath + "\\image" + idProduct + ".png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await updateFile.CopyToAsync(stream);
                        results = true;
                    }
                    dataImage.nameImage = imagePath;
                    _context.Images.Add(dataImage);
                    await _context.SaveChangesAsync();
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
            var dataProduct = _context.Product.FirstOrDefault(a => a.Id == id);
            if (dataProduct == null)
            {
                return Ok(new Response { Status = "500", Message = "Entity set 'Web_Doan_2023Context.Product'  is null.!" });
            }
            dataProduct.IsDelete= true;
            _context.Entry(dataProduct).State = EntityState.Modified;
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
