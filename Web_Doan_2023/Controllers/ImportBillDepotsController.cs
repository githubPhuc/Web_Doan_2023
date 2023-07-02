using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportBillDepotsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public ImportBillDepotsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/ImportBillDepots
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? codeBill, string?nameDepot)
        {
            var list = (from a in db_.ImportBillDepot
                        select new
                        {
                            Id = a.Id,
                            codeBill = a.codeBill,
                            IdDepot= a.IdDepot,
                            NameDepot = (a.IdDepot==0?"":db_.Depot.Where(c=>c.Id==a.IdDepot).FirstOrDefault().nameDepot),
                            Quantity=a.Quantity ,
                            Price = a.Price,
                            CreatedDate =a.CreatedDate,
                            UpdatedDate=a.UpdatedDate,
                            UserCreate =a.UserCreate,
                            UserUpdate=a.UserUpdate,
                            Status=a.Status,
                            IsAcceptance=a.IsAcceptance,

                        }).ToList();
            var data = list.Where(a => 
            (codeBill == "" || codeBill == null || a.codeBill.ToUpper().Contains(codeBill.ToUpper()) &&
            (nameDepot == "" || nameDepot == null || a.NameDepot.ToUpper().Contains(nameDepot.ToUpper())
            ))).ToList();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListDetail")]
        public async Task<ActionResult> GetListDetail(int Id)
        {
            var data = (from a in db_.ImportBillDepotDetail
                        where a.BillId ==Id
                        select new
                        {
                            Id = a.Id,
                            idProduct = a.idProduct,
                            NameProduct = (a.idProduct == 0 ? "" : db_.Product.Where(c => c.Id == a.idProduct).FirstOrDefault().nameProduct),
                            BillId=a.BillId,
                            quantity=a.Quantity,
                            price=a.price,

                        }).ToList();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListOnID")]
        public async Task<ActionResult> GetListOnID(int id)
        {
            var data = db_.ImportBillDepot.Where(a=>a.Id==id).ToList();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [NonAction]
        public string CreateNewCodeBillImportDepot(int ID)
        {
            DateTime date = DateTime.Now;
            string day = String.Format("{0:D2}", date.Day);
            string month = String.Format("{0:D2}", date.Month);
            string year = String.Format("{0:D4}", date.Year);
            string Code = day + month + year + "-" + ID.ToString();
            return Code;
        }

        // POST: api/ImportBillDepots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Insert")]
        public async Task<ActionResult> Insert(ImportBillDepot model)
        {

            try
            {
                var data = new ImportBillDepot()
                {
                    codeBill = "",
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Price = 0,
                    IsAcceptance = model.IsAcceptance,
                    UserCreate = model.UserCreate,
                    Status = isStatus.WaitingForApproval,
                };
                db_.ImportBillDepot.Add(data);
                await db_.SaveChangesAsync();
                string code = CreateNewCodeBillImportDepot(data.Id);
                data.codeBill = code;
                db_.Entry(data).State = EntityState.Modified;
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Insert product " + model.codeBill + " successfully!" });

            }
            catch (Exception ex)
            {
                return Ok(new Response { Status = "Failed", Message = ex.Message });
            }
        }
        [HttpPost("InsertDetail")]
        public async Task<ActionResult> InsertDetail(ImportBillDepotDetail model)
        {

            try
            {
                var checkBill = db_.ImportBillDepot.Where(a=>a.Id== model.BillId).FirstOrDefault();
                if (checkBill == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Bill import depot is null" });
                }
                else
                {
                    var data = new ImportBillDepotDetail()
                    {
                        BillId = model.BillId,
                        idProduct = model.idProduct,
                        price = model.price,
                        Quantity = model.Quantity,
                    };
                    db_.ImportBillDepotDetail.Add(data);
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Insert successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response { Status = "Failed", Message = ex.Message });
            }
        }

        // DELETE: api/ImportBillDepots/5
        [HttpPost("DeleteDetail")]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var data = db_.ImportBillDepotDetail.FirstOrDefault(a => a.Id == id);
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Bill detail is null!" });
            }// kiểm tra 
            try
            {
                db_.ImportBillDepotDetail.Remove(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Delete successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Status = "Failed", Message = ex.Message });
            }
        }// chưa kiểm tra
        [HttpPost("DeleteBill")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var check = db_.ImportBillDepotDetail.Where(a => a.BillId == id).ToList();
            if (check.Count()>0)
            {
                return Ok(new Response { Status = "Failed", Message = "Import depot details exist!" });
            }// kiểm tra 
            try
            {
                var data = db_.ImportBillDepot.Where(a => a.Id == id && a.IsAcceptance==false).FirstOrDefault();
                if (data == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Import depot Acceptance!" });
                }
                else
                {
                    db_.ImportBillDepot.Remove(data);
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Delete successfully" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response { Status = "Failed", Message = ex.Message });
            }
        }
        [HttpPost("acceptance")]
        public async Task<IActionResult> acceptance(int id)
        {
            
            try
            {
                var data = db_.ImportBillDepot.Where(a => a.Id == id).FirstOrDefault();
                data.IsAcceptance = true;
                db_.Entry(data).State = EntityState.Modified;
                await db_.SaveChangesAsync();
                var data_billDetail = db_.ImportBillDepotDetail.Where(a => a.BillId == id).ToList();
                if(data_billDetail.Count()>0)
                {
                    try
                    {
                        foreach (var item in data_billDetail)
                        {
                            var check_ProductDepot = db_.productDepot.Where(a => a.idProduct == item.idProduct&& a.idDepot==data.IdDepot).FirstOrDefault();
                            if(check_ProductDepot == null)
                            {
                                var data_productDepot = new productDepot()
                                {
                                    idProduct = item.idProduct,
                                    idDepot = data.IdDepot,
                                    QuantityProduct = item.Quantity,
                                };
                                db_.productDepot.Add(data_productDepot);
                                db_.SaveChanges();
                            }
                            else
                            {
                                check_ProductDepot.QuantityProduct = item.Quantity;
                                db_.Entry(check_ProductDepot).State = EntityState.Modified;
                                await db_.SaveChangesAsync();
                            }
                        }
                        return Ok(new Response { Status = "Success", Message = "successfully" });
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response { Status = "Failed", Message = ex.Message });
                    }

                }
                return Ok(new Response { Status = "Failed", Message = "successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Status = "Failed", Message = ex.Message });
            }
        }
        private bool ImportBillDepotExists(int id)
        {
            return (db_.ImportBillDepot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
