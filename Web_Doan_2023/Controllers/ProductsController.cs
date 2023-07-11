using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;
        private readonly IWebHostEnvironment _environment;
        public ProductsController(Web_Doan_2023Context context, IWebHostEnvironment environment)
        {
            db_ = context;
            _environment = environment;
        }

       
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? nameProduct, string? nameProduce, string? nameRam, string? nameCpu, string? nameDisplay, string? nameColor, string? nameCard)
        {
            var list = (from a in db_.Product
                        where a.IsDelete == false 
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            idCategory = a.idCategory,
                            nameCategory = (a.idCategory == 0 || a.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == a.idCategory).FirstOrDefault().nameCategory,
                            idProducer = a.idProducer,
                            nameProduce = (a.idProducer == 0 || a.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == a.idProducer).FirstOrDefault().nameProduce,
                            price = a.price,
                            RamProduct = a.RamProduct,
                            RamName = (a.RamProduct == 0 || a.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == a.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = a.SSDProduct,
                            SSDName = (a.SSDProduct == 0 || a.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == a.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = a.CPUProduct,
                            CPUName = (a.CPUProduct == 0 || a.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == a.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = a.DisplayProduct,
                            DisplayName = (a.DisplayProduct == 0 || a.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == a.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = a.ColorProduct,
                            ColorName = (a.ColorProduct == 0 || a.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == a.ColorProduct).FirstOrDefault().Name,
                            portConnection = a.portConnection,
                            CardDisplay = a.CardDisplay,
                            CardDisplayName = (a.CardDisplay == 0 || a.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == a.CardDisplay).FirstOrDefault().Name,
                            mainboar = a.MainProduct,
                            AccessoriesIncluded = a.AccessoriesIncluded,
                            Status = a.Status,
                            idSale = a.idSale,
                            SaleName = (a.idSale == 0 || a.idSale == null) ? "" : db_.Sale.Where(c => c.Id == a.idSale).FirstOrDefault().nameSale,

                            IsDelete = a.IsDelete,
                        }).ToArray();

            var data = list.Where(a => (
                                (nameProduct == "" || nameProduct == null || a.nameproduct.ToUpper().Contains(nameProduct.ToUpper())) &&
                                (nameProduce == "" || nameProduce == null || a.nameProduce.ToUpper().Contains(nameProduce.ToUpper())) &&
                                (nameRam == "" || nameRam == null || a.RamName.ToUpper().Contains(nameRam.ToUpper())) &&
                                (nameCpu == "" || nameCpu == null || a.CPUName.ToUpper().Contains(nameCpu.ToUpper())) &&
                                (nameDisplay == "" || nameDisplay == null || a.DisplayName.ToUpper().Contains(nameDisplay.ToUpper())) &&
                                (nameColor == "" || nameColor == null || a.ColorName.ToUpper().Contains(nameColor.ToUpper())) &&
                                (nameCard == "" || nameCard == null || a.CardDisplayName.ToUpper().Contains(nameCard.ToUpper()))

            )
            ).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListUser")]
        public async Task<ActionResult> GetListUser(string? nameProduct, string? nameProduce, string? nameRam, string? nameCpu, string? nameDisplay, string? nameColor, string? nameCard)
        {
            var list = (from a in db_.Product
                        where a.IsDelete == false && a.Status == true
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            idCategory = a.idCategory,
                            nameCategory = (a.idCategory == 0 || a.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == a.idCategory).FirstOrDefault().nameCategory,
                            idProducer = a.idProducer,
                            nameProduce = (a.idProducer == 0 || a.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == a.idProducer).FirstOrDefault().nameProduce,
                            price = a.price,
                            RamProduct = a.RamProduct,
                            RamName = (a.RamProduct == 0 || a.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == a.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = a.SSDProduct,
                            SSDName = (a.SSDProduct == 0 || a.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == a.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = a.CPUProduct,
                            CPUName = (a.CPUProduct == 0 || a.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == a.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = a.DisplayProduct,
                            DisplayName = (a.DisplayProduct == 0 || a.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == a.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = a.ColorProduct,
                            ColorName = (a.ColorProduct == 0 || a.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == a.ColorProduct).FirstOrDefault().Name,
                            portConnection = a.portConnection,
                            CardDisplay = a.CardDisplay,
                            CardDisplayName = (a.CardDisplay == 0 || a.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == a.CardDisplay).FirstOrDefault().Name,
                            mainboar = a.MainProduct,
                            AccessoriesIncluded = a.AccessoriesIncluded,
                            Status = a.Status,
                            idSale = a.idSale,
                            SaleName = (a.idSale == 0 || a.idSale == null) ? "" : db_.Sale.Where(c => c.Id == a.idSale).FirstOrDefault().nameSale,

                            IsDelete = a.IsDelete,
                        }).ToArray();

            var data = list.Where(a => (
                                (nameProduct == "" || nameProduct == null || a.nameproduct.ToUpper().Contains(nameProduct.ToUpper())) &&
                                (nameProduce == "" || nameProduce == null || a.nameProduce.ToUpper().Contains(nameProduce.ToUpper())) &&
                                (nameRam == "" || nameRam == null || a.RamName.ToUpper().Contains(nameRam.ToUpper())) &&
                                (nameCpu == "" || nameCpu == null || a.CPUName.ToUpper().Contains(nameCpu.ToUpper())) &&
                                (nameDisplay == "" || nameDisplay == null || a.DisplayName.ToUpper().Contains(nameDisplay.ToUpper())) &&
                                (nameColor == "" || nameColor == null || a.ColorName.ToUpper().Contains(nameColor.ToUpper())) &&
                                (nameCard == "" || nameCard == null || a.CardDisplayName.ToUpper().Contains(nameCard.ToUpper()))

            )
            ).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }

        // GET: api/Products/5
        [HttpGet("GetProductById")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var data = (from a in db_.Product
                        where a.IsDelete== false&& a.Id==id
                        select a).FirstOrDefault();
            return Ok(new
            {
                data = data
            });
        }
        // GET: api/Products/5
        [HttpGet("GetCBX")]
        public async Task<ActionResult> GetCBX()
        {
            var data = (from a in db_.Product
                        where a.IsDelete == false
                        select a).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetProductOnIdDetail")]
        public async Task<ActionResult> GetProductOnIdDetail(int id)
        {
            var data = (from a in db_.Product
                        where a.IsDelete == false && a.Id == id
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            idCategory = a.idCategory,
                            nameCategory = (a.idCategory == 0 || a.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == a.idCategory).FirstOrDefault().nameCategory,
                            idProducer = a.idProducer,
                            nameProduce = (a.idProducer == 0 || a.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == a.idProducer).FirstOrDefault().nameProduce,
                            price = a.price,
                            RamProduct = a.RamProduct,
                            RamName = (a.RamProduct == 0 || a.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == a.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = a.SSDProduct,
                            SSDName = (a.SSDProduct == 0 || a.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == a.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = a.CPUProduct,
                            CPUName = (a.CPUProduct == 0 || a.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == a.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = a.DisplayProduct,
                            DisplayName = (a.DisplayProduct == 0 || a.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == a.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = a.ColorProduct,
                            ColorName = (a.ColorProduct == 0 || a.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == a.ColorProduct).FirstOrDefault().Name,
                            portConnection = a.portConnection,
                            CardDisplay = a.CardDisplay,
                            CardDisplayName = (a.CardDisplay == 0 || a.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == a.CardDisplay).FirstOrDefault().Name,
                            mainboar = a.MainProduct,
                            AccessoriesIncluded = a.AccessoriesIncluded,
                            Status = a.Status,
                            idSale = a.idSale,
                            SaleName = (a.idSale == 0 || a.idSale == null) ? "null" : db_.Sale.Where(c => c.Id == a.idSale).FirstOrDefault().nameSale,

                            IsDelete = a.IsDelete,
                        }).FirstOrDefault();


            return Ok(new
            {
                data = data
            });
        }
        [HttpGet("GetProductOnCategory")]
        public async Task<ActionResult> GetProductOnCategory(int id)
        {
            var data = (from a in db_.Product
                        where a.IsDelete == false && a.idCategory == id
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            idCategory = a.idCategory,
                            nameCategory = (a.idCategory == 0 || a.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == a.idCategory).FirstOrDefault().nameCategory,
                            idProducer = a.idProducer,
                            nameProduce = (a.idProducer == 0 || a.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == a.idProducer).FirstOrDefault().nameProduce,
                            price = a.price,
                            RamProduct = a.RamProduct,
                            RamName = (a.RamProduct == 0 || a.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == a.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = a.SSDProduct,
                            SSDName = (a.SSDProduct == 0 || a.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == a.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = a.CPUProduct,
                            CPUName = (a.CPUProduct == 0 || a.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == a.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = a.DisplayProduct,
                            DisplayName = (a.DisplayProduct == 0 || a.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == a.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = a.ColorProduct,
                            ColorName = (a.ColorProduct == 0 || a.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == a.ColorProduct).FirstOrDefault().Name,
                            portConnection = a.portConnection,
                            CardDisplay = a.CardDisplay,
                            CardDisplayName = (a.CardDisplay == 0 || a.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == a.CardDisplay).FirstOrDefault().Name,
                            mainboar = a.MainProduct,
                            AccessoriesIncluded = a.AccessoriesIncluded,
                            Status = a.Status,
                            idSale = a.idSale,
                            SaleName = (a.idSale == 0 || a.idSale == null) ? "null" : db_.Sale.Where(c => c.Id == a.idSale).FirstOrDefault().nameSale,

                            IsDelete = a.IsDelete,
                        }).ToList();

            
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetProductOnProducer")]
        public async Task<ActionResult<Product>> GetProductOnProducer(int id)
        {
            var data = (from a in db_.Product
                        where a.IsDelete == false && a.idProducer == id
                        select new
                        {
                            Id = a.Id,
                            codeproduct = a.codeProduct,
                            nameproduct = a.nameProduct,
                            Descriptionproduct = a.Description,
                            idCategory = a.idCategory,
                            nameCategory = (a.idCategory == 0 || a.idCategory == null) ? "null" : db_.CategoryProduct.Where(c => c.Id == a.idCategory).FirstOrDefault().nameCategory,
                            idProducer = a.idProducer,
                            nameProduce = (a.idProducer == 0 || a.idProducer == null) ? "null" : db_.Producer.Where(c => c.Id == a.idProducer).FirstOrDefault().nameProduce,
                            price = a.price,
                            RamProduct = a.RamProduct,
                            RamName = (a.RamProduct == 0 || a.RamProduct == null) ? "null" : db_.RamProduct.Where(c => c.Id == a.RamProduct).FirstOrDefault().nameRam,
                            SSDProduct = a.SSDProduct,
                            SSDName = (a.SSDProduct == 0 || a.SSDProduct == null) ? "null" : db_.SsdProduct.Where(c => c.Id == a.SSDProduct).FirstOrDefault().Name,
                            CPUProduct = a.CPUProduct,
                            CPUName = (a.CPUProduct == 0 || a.CPUProduct == null) ? "null" : db_.CpuProduct.Where(c => c.Id == a.CPUProduct).FirstOrDefault().Name,
                            DisplayProduct = a.DisplayProduct,
                            DisplayName = (a.DisplayProduct == 0 || a.DisplayProduct == null) ? "null" : db_.DisplayProduct.Where(c => c.Id == a.DisplayProduct).FirstOrDefault().Name,
                            ColorProduct = a.ColorProduct,
                            ColorName = (a.ColorProduct == 0 || a.ColorProduct == null) ? "null" : db_.ColorProduct.Where(c => c.Id == a.ColorProduct).FirstOrDefault().Name,
                            portConnection = a.portConnection,
                            CardDisplay = a.CardDisplay,
                            CardDisplayName = (a.CardDisplay == 0 || a.CardDisplay == null) ? "null" : db_.CardDisplay.Where(c => c.Id == a.CardDisplay).FirstOrDefault().Name,
                            mainboar = a.MainProduct,
                            AccessoriesIncluded = a.AccessoriesIncluded,
                            Status = a.Status,
                            idSale = a.idSale,
                            SaleName = (a.idSale == 0 || a.idSale == null) ? "null" : db_.Sale.Where(c => c.Id == a.idSale).FirstOrDefault().nameSale,

                            IsDelete = a.IsDelete,
                        }).ToList();


            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("ListImages")]
        public async Task<ActionResult> ListImages(int idProduct)
        {
            var data = (from a in db_.Images
                        where a.idProduct ==idProduct
                       select new
                       {
                           id=a.Id,
                           idProduct = a.idProduct,
                           nameProduct = (a.idProduct==0||a.idProduct==null)?"":db_.Product.Where(c=>c.Id==a.idProduct).FirstOrDefault().nameProduct,
                           PathImage = a.PathImage,
                           nameImage = a.nameImage,
                       }).ToList();
           // var data = list.Where(a => (nameProduct == null || nameProduct == "" || a.nameProduct.ToUpper().Contains(nameProduct.ToUpper()))).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }


        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Insert")]
        public async Task<ActionResult<Product>> Insert([FromBody] ProductModel model)
        {
            // Mã sản phẩm  gồm( Loại - Nhà sản xuất - màu -id sản phẩm)
            string codeproduct = "";
            var dataCate = await db_.CategoryProduct.Where(a=>a.Id== model.idCategory).FirstOrDefaultAsync();
            if (dataCate != null)
            {
                codeproduct = dataCate.codeCategory + "-";
            }
            var dataProducer = await db_.Producer.Where(a => a.Id == model.idProducer).FirstOrDefaultAsync();
            if(dataProducer != null)
            {
                codeproduct += dataProducer.codeProduce + "-";
            }
            var dataCol = db_.ColorProduct.Where(a=>a.Id==model.ColorProduct).FirstOrDefault();
            if(dataCol != null)
            {
                codeproduct += dataCol.code;
            }
            if(codeproduct.Length!=11)
            {
                return Ok(new Response { Status = "Failed", Message = "failed to initialize item code! "+ codeproduct });
            }
            else
            {
                try
                {
                    //Chuwa lafm saong
                    var data = new Product()
                    {
                        codeProduct = codeproduct,
                        nameProduct = model.nameProduct,
                        Description = model.Description,
                        idCategory = model.idCategory,
                        idProducer = model.idProducer,
                        RamProduct = model.RamProduct,
                        SSDProduct = model.SSDProduct,
                        CPUProduct = model.CPUProduct,
                        MainProduct = model.MainProduct,//
                        DisplayProduct = model.DisplayProduct,
                        ColorProduct = model.ColorProduct,
                        portConnection = model.portConnection,
                        CardDisplay = model.CardDisplay,
                        AccessoriesIncluded = model.AccessoriesIncluded,
                        Status = false,
                        idSale = null,
                        IsDelete = false,
                    };
                    db_.Product.Add(data);
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Insert product " + model.nameProduct + " successfully!" });

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromBody] ProductModel model, int id)
        {
            // Mã sản phẩm  gồm( Loại - Nhà sản xuất - màu -id sản phẩm)
            string codeproduct = "";
            var dataCate = await db_.CategoryProduct.Where(a => a.Id == model.idCategory).FirstOrDefaultAsync();
            if (dataCate != null)
            {
                codeproduct = dataCate.codeCategory + "-";
            }
            var dataProducer = await db_.Producer.Where(a => a.Id == model.idProducer).FirstOrDefaultAsync();
            if (dataProducer != null)
            {
                codeproduct += dataProducer.codeProduce + "-";
            }
            var dataCol = db_.ColorProduct.Where(a => a.Id == model.ColorProduct).FirstOrDefault();
            if (dataCol != null)
            {
                codeproduct += dataCol.code;
            }
            if (codeproduct.Length != 11)
            {
                return Ok(new Response { Status = "Failed", Message = "failed to initialize item code! " + codeproduct });
            }
            else
            {
                try
                {
                    var data = db_.Product.Where(a => a.Id == id).FirstOrDefault();
                    data.codeProduct = codeproduct;
                    data.Description = model.Description;
                    data.nameProduct = model.nameProduct;
                    data.Description = model.Description;
                    data.idCategory = model.idCategory;
                    data.idProducer = model.idProducer;
                    data.RamProduct = model.RamProduct;
                    data.SSDProduct = model.SSDProduct;
                    data.CPUProduct = model.CPUProduct;
                    data.MainProduct = model.MainProduct;
                    data.DisplayProduct = model.DisplayProduct;
                    data.ColorProduct = model.ColorProduct;
                    data.portConnection = model.portConnection;
                    data.CardDisplay = model.CardDisplay;
                    data.AccessoriesIncluded = model.AccessoriesIncluded;
                    data.Status = false;
                    data.idSale = model.idSale;
                    data.IsDelete = false;
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Update product " + model.nameProduct + " successfully!" });

                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }
        }
        [HttpPost("uploadImage")]
        public async Task<ActionResult> uploadImage(int id)
        {
            try
            {
                var formCollection = await  Request.ReadFormAsync();
                var file_ = formCollection.Files.First();
                var dataProduct = db_.Product.FirstOrDefault(a => a.Id == id);
                if (dataProduct == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Product is null!" });
                }//Kiễm tra sự tồn tại của sản phẩm 
                else
                {
                    if (file_!= null)
                    {
                        string fileName = "Product-" + dataProduct.Id.ToString();
                        string filePath = GetFilePath(fileName);

                        if (!System.IO.Directory.Exists(filePath))
                        {
                            System.IO.Directory.CreateDirectory(filePath);
                        }

                        if (System.IO.File.Exists(filePath))
                        {

                            System.IO.File.Delete(filePath);
                        }
                        string imageName = (Guid.NewGuid().ToString())+ ".png";
                        var dataImage = new Images();//khởi tạo data image
                        string imagePath = filePath + "\\"+ imageName;
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (FileStream stream = System.IO.File.Create(imagePath))
                        {
                            await file_.CopyToAsync(stream);
                            
                        }
                        dataImage.idProduct = id;
                        dataImage.nameImage = imageName;
                        dataImage.PathImage = GetImagebycode(imageName, fileName);
                        db_.Images.Add(dataImage);
                        await db_.SaveChangesAsync();

                        return Ok(new Response { Status = "Success", Message = "Upload image successfully!" });
                    }
                    else
                    {
                        return Ok(new Response { Status = "Failed", Message = "File is null!" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost("deleteImage")]
        public async Task<ActionResult> deleteImage(int id,int idProduct)
        {
            bool isSuccess = false;
            var data = db_.Images.Where(a => a.Id == id).FirstOrDefault();
            if (data != null)
            {
                string fileName = "Product-" + idProduct;
                string filePath = GetFilePath(fileName);
                string imagePath = filePath + "\\" + data.nameImage;
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    isSuccess=true;
                }
                if (isSuccess == true)
                {
                    db_.Images.Remove(data);
                    db_.SaveChanges();
                    return Ok(new Response { Status = "Success", Message = "Delete image successfully!" });
                }
                return Ok(new Response { Status = "Failed", Message = "Delete file image Failed!" });
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "image not exist!" });
            }
        }
       

        // DELETE: api/Products/5
        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
           var checkDepot =db_.productDepot.Where(x => x.idProduct == id).ToList();
            if(checkDepot.Count() == 0)
            {
                var dataProduct = db_.Product.FirstOrDefault(a => a.Id == id);
                if (dataProduct == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Product is null!" });
                }
                else
                {
                    dataProduct.IsDelete = true;
                    db_.Entry(dataProduct).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Upload image successfully!" });
                }
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "Please delete in the import depot!" });
            }
            
        }

        
        private bool ProductExists(int id)
        {
            return (db_.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [NonAction]
        private string GetFilePath(string productCode)
        {
            return this._environment.WebRootPath+"\\image\\product\\"+ productCode;
        }
        [NonAction]
        private string GetImagebycode(string image,string filename)
        {
            string hosturl = "https://localhost:7109";
            string Filepath = GetFilePath(filename) + "/" + image;
            if (System.IO.File.Exists(Filepath))
                return hosturl + "/image/product/"+filename+"/" + image;
            else
                return hosturl + "/image/product/No-Image.png";
        }
    }
}
