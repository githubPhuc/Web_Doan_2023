using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
                    IdDepot = model.IdDepot,
                    codeBill = "",
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Price = 0,
                    Quantity = 0,
                    IsAcceptance = false,
                    UserCreate = model.UserCreate,
                    Status = isStatus.WaitingForApproval,
                };
                db_.ImportBillDepot.Add(data);
                await db_.SaveChangesAsync();
                string code = CreateNewCodeBillImportDepot(data.Id);
                data.codeBill = code;
                db_.Entry(data).State = EntityState.Modified;
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Import Bill Depot " + model.codeBill + " successfully!" });

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
                if (checkBill.IsAcceptance == true)
                {
                    return Ok(new Response { Status = "Failed", Message = "Import depot Acceptance!" });
                }
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
                    checkBill.Quantity += model.Quantity;
                    checkBill.Price += model.price;
                    db_.Entry(checkBill).State = EntityState.Modified;
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
                var checkBill = db_.ImportBillDepot.FirstOrDefault(a => a.Id == data.BillId);
                if(checkBill.IsAcceptance==true)
                {
                    return Ok(new Response { Status = "Failed", Message = "Import depot Acceptance!" });
                }
                else
                {
                    checkBill.Quantity -= data.Quantity;
                    checkBill.Price -= data.price;
                    db_.Entry(checkBill).State = EntityState.Modified;
                    db_.ImportBillDepotDetail.Remove(data);
                    await db_.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Delete successfully" });
                }
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
                var data = db_.ImportBillDepot.Where(a => a.Id == id ).FirstOrDefault();
                if (data.IsAcceptance == true)
                {
                    return Ok(new Response { Status = "Failed", Message = "Import depot Acceptance!" });
                }
                if (data == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Warehouse import does not exist!" });
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
        public async Task<IActionResult> acceptance(int id,string user)
        {
            bool isSuccess = false;
            try
            {
                var data = db_.ImportBillDepot.Where(a => a.Id == id).FirstOrDefault();
                if(data.IsAcceptance==true)
                {
                    return Ok(new Response { Status = "Failed", Message = " Order approved" });
                }
                else
                {
                    data.IsAcceptance = true;
                    data.UserUpdate = user;
                    data.Status = isStatus.warehouseimported;
                    data.UpdatedDate = DateTime.Now;
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    var data_billDetail = db_.ImportBillDepotDetail.Where(a => a.BillId == id).ToList();
                    if (data_billDetail.Count() > 0)
                    {
                        try
                        {
                            foreach (var item in data_billDetail)
                            {
                                var check_ProductDepot = db_.productDepot.Where(a => a.idProduct == item.idProduct && a.idDepot == data.IdDepot).FirstOrDefault();
                                if (check_ProductDepot == null)
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
                            isSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            return Ok(new Response { Status = "Failed", Message = ex.Message });
                        }

                    }
                    isSuccess = true;
                }
                if(isSuccess == true)
                {
                    string content = "Mail xác nhận đơn hàng nhập kho<br>";
                    content += "Kính gửi:Anh/Chị<br> Chứng từ nhập kho có mã <font color='blue'> " + data.codeBill + " </font> đả được user "+user+" xác nhận <br>";
                    content += "Kiểm tra lại tại chương trình grament <br>";
                    content += "<a href=\"http://localhost:4200/Login\">Chuyển tiếp</a>";
                    string _from = "0306191061@caothang.edu.vn";
                    string _subject = "XÁC NHẬN ĐƠN NHẬP KHO";
                    string _body = content;
                    string _gmail = "0306191061@caothang.edu.vn";
                    string _password = "285728207";
                    MailMessage message = new MailMessage(_from, "ptranninh@gmail.com", _subject, _body);
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;
                    message.IsBodyHtml = true;
                    message.ReplyToList.Add(new MailAddress(_from));
                    message.Sender = new MailAddress(_from);

                    using var smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(_gmail, _password);

                    try
                    {
                        await smtpClient.SendMailAsync(message);
                        return Ok(new Response { Status = "Success", Message = "acceptance successfully" });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return Ok(new Response { Status = "Failed", Message = "Send mail Failed" });
                    }
                }
                else
                {
                    return Ok(new Response { Status = "Failed", Message = "acceptance Failed" });
                }
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
