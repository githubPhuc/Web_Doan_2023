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
    public class CategoryProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public CategoryProductsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/CategoryProducts
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string ? name)
        {
            var data = await (from a in db_.CategoryProduct
                              where (
                                       (name == null || name == "" || a.nameCategory.ToUpper().Contains(name.ToUpper()))
                                    )
                              select new
                              {
                                  Id = a.Id,
                                  codeCategory= a.codeCategory,
                                  nameCategory = a.nameCategory,
                                  Status = a.Status,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }

        // POST: api/CategoryProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insert")]
        public async Task<ActionResult> insert(string codeCategory,string nameCategory)
        {
            var check = await db_.CategoryProduct.Where(a => a.codeCategory == codeCategory).FirstOrDefaultAsync();
            if (check != null)
            {
                return Ok(new Response
                {
                    Status = "Failed",
                    Message = "Category is exist!"
                });
            }
            else
            {
                var data = new CategoryProduct()
                {
                    codeCategory = codeCategory,
                    nameCategory = nameCategory,
                    Status = true,
                };
                db_.CategoryProduct.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Insert category " + nameCategory + " successfully!" });
            }
        }

        // DELETE: api/CategoryProducts/5
        [HttpPost("delete")]
        public async Task<IActionResult> delete(int id)
        {
            var categoryProduct = await db_.CategoryProduct.FindAsync(id);
            if (categoryProduct == null)
            {
                return Ok(new Response
                {
                    Status = "Failed",
                    Message = "Category not exist!"
                });
            }
            var dataProduct = db_.Product.Where(a => a.idCategory == id).ToList();
            if(dataProduct.Count > 0) 
            { 
                return Ok(new Response { 
                    Status = "Failed", 
                    Message = "The product exists in the database!Unable to delete category product" 
                }); 
            }
            db_.CategoryProduct.Remove(categoryProduct);
            await db_.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Delete category product successfully!" });
        }

        private bool CategoryProductExists(int id)
        {
            return (db_.CategoryProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
