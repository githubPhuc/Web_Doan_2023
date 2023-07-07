using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillOfSalesController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public BillOfSalesController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/BillOfSales
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? code)
        {
            var list = (from a in db_.BillOfSale
                        where a.IsDelete == false
                        select new
                        {
                            Id = a.Id,
                            code = a.code,
                            phone = a.phone,
                            Address = a.Address,
                            sumPrice = a.sumPrice,
                            sumQuantity = a.sumQuantity,
                            createDate = a.createDate,
                            updateDate = a.updateDate,
                            deleteDate = a.deleteDate,
                            UsernameCreate = a.UsernameCreate,
                            UsernameDelete = a.UsernameDelete,
                            UsernameUpdate = a.UsernameUpdate,
                            IsDelete = a.IsDelete,
                            StatusBill = a.StatusBill,
                            StatusCode = a.StatusCode,
                        }).ToList();
            var data = list.Where(a=>(code==null||code==""||a.code.ToUpper().Contains(code.ToUpper()))).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        
        [NonAction]
        public string CreateNewCodeBillOfSale()
        {
            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "1234567890";
            string allCharacters = UpperCase + Digits;
            Random r = new Random();
            String password = "";
            for (int i = 0; i < 6; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                {
                    password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
                }
            }
            DateTime date = DateTime.Now;
            string day = String.Format("{0:D2}", date.Day);
            string month = String.Format("{0:D2}", date.Month);
            string year = String.Format("{0:D4}", date.Year);
            string Code = day + month + year + "-" + password;
            return Code;
        }
        [HttpPost("InsertBill")]
        public async Task<ActionResult> InsertBill([FromBody]BillOfSale model)
        {
            var checkUser = db_.Users.FirstOrDefault(a => a.UserName == model.UsernameCreate);
            if(checkUser == null)
            {
                return Ok(new Response { Status = "Failed", Message = " Ordering users not found" });
            }
            else {
                string code = CreateNewCodeBillOfSale();
                var data = new BillOfSale()
                {
                    code = code,
                    Address = model.Address,
                    phone = model.phone,
                    UsernameCreate = model.UsernameCreate,
                    createDate = DateTime.Now,
                    sumQuantity = 0,
                    sumPrice = 0,
                    StatusBill = isStatus.waitForConfirmation,
                    StatusCode = true,

                };
                db_.BillOfSale.Add(data);
                await db_.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = " Create bill of sale successfully" });
            }
        }
        [HttpPost("CreateDetailBillOfSale")]
        public async Task<ActionResult> CreateDetailBillOfSale(string user,string Address,string phone)
        {
            var dataUser = db_.Users.Where(a => a.UserName == user).FirstOrDefault();
            bool isSuccess = false;
            var dataCart = db_.CartProduct.Where(a => a.Username == user).ToList();
            if (dataCart.Count() > 0)
            {
                var checkBill = db_.BillOfSale.FirstOrDefault(a => a.UsernameCreate == user&& a.IsDelete == false);// baằng user và đả xóa
                if (checkBill != null)
                {
                    checkBill.UsernameUpdate = user;
                    checkBill.updateDate = DateTime.Now;
                    checkBill.phone = phone;
                    checkBill.Address = Address;
                    db_.Entry(checkBill).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    foreach (var item in dataCart)
                    {
                        var dataProductDepot = db_.productDepot.Where(a => a.idProduct == item.ProductId).FirstOrDefault();
                        if (dataProductDepot == null)
                        {
                            return Ok(new Response { Status = "Failed", Message = " Products not yet in stock" });
                        }
                        else
                        {
                            if (dataProductDepot.QuantityProduct < item.Quantity)
                            {
                                return Ok(new Response { Status = "Failed", Message = "Number of inventory is not enough" });
                            }
                            else
                            {
                                var checkDetailBill = db_.BillOfSaleDetail.Where(a => a.Idproduct == item.ProductId).FirstOrDefault();
                                if (checkDetailBill == null)
                                {
                                    var data = new BillOfSaleDetail()
                                    {
                                        IdBill = checkBill.Id,
                                        Idproduct = item.ProductId,
                                        Quantity = item.Quantity,
                                        Price = (decimal)item.Price!,
                                    };
                                    db_.BillOfSaleDetail.Add(data);
                                    await db_.SaveChangesAsync();
                                    isSuccess = true;
                                }
                                else
                                {
                                    checkDetailBill.Quantity += item.Quantity;
                                    db_.Entry(checkDetailBill).State = EntityState.Modified;
                                    await db_.SaveChangesAsync();
                                    isSuccess = true;
                                }

                            }
                        }
                    }
                    if (isSuccess == true)
                    {
                        string content = "mail sác nhận đơn hàng<br>";
                        content += "Kính gửi:Anh/Chị<br> Đả tin tưởng và ủng hộ.<br> Đây là mã đơn hàng của bạn <font color='blue'> " + checkBill.code + " </font>";
                        content += "<br> Thời gian đặt đơn là " + DateTime.Now + " xác nhận <br>";
                        content += "<br>Vui lòng xác nhận đơn hàng<br>";
                        content += "<a href=\"http://localhost:4200/Login\">Xác nhận đơn hàng</a>";
                        string _from = "0306191061@caothang.edu.vn";
                        string _subject = "Shop nhận đơn";
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
                        return Ok(new Response { Status = "Failed", Message = " Sales invoice generation failed" });
                    }
                }
                else
                {
                    try
                    {
                        string code = CreateNewCodeBillOfSale();
                        var dataBill = new BillOfSale()
                        {
                            code = code,
                            Address = Address,
                            phone = phone,
                            UsernameCreate = user,
                            createDate = DateTime.Now,
                            sumQuantity = 0,
                            sumPrice = 0,
                            StatusBill = isStatus.waitForConfirmation,
                            StatusCode = true,
                            IsDelete = false,

                        };
                        db_.BillOfSale.Add(dataBill);
                        await db_.SaveChangesAsync();
                        foreach (var item in dataCart)
                        {
                            var dataProductDepot = db_.productDepot.Where(a => a.idProduct == item.ProductId).FirstOrDefault();
                            if (dataProductDepot == null)
                            {
                                return Ok(new Response { Status = "Failed", Message = " Products not yet in stock" });
                            }
                            else
                            {
                                if (dataProductDepot.QuantityProduct < item.Quantity)
                                {
                                    return Ok(new Response { Status = "Failed", Message = "Number of inventory is not enough" });
                                }
                                else
                                {
                                    var checkDetailBill = db_.BillOfSaleDetail.Where(a => a.Idproduct == item.ProductId).FirstOrDefault();
                                    if (checkDetailBill == null)
                                    {
                                        var data = new BillOfSaleDetail()
                                        {
                                            IdBill = dataBill.Id,
                                            Idproduct = item.ProductId,
                                            Quantity = item.Quantity,
                                            Price = (decimal)item.Price!,
                                        };
                                        db_.BillOfSaleDetail.Add(data);
                                        await db_.SaveChangesAsync();
                                        isSuccess = true;
                                    }
                                    else
                                    {
                                        checkDetailBill.Quantity += item.Quantity;
                                        db_.Entry(checkDetailBill).State = EntityState.Modified;
                                        await db_.SaveChangesAsync();
                                        isSuccess = true;
                                    }
                                   
                                }
                            }
                        }
                        if(isSuccess== true)
                        {
                            string content = "mail sác nhận đơn hàng<br>";
                            content += "Kính gửi:Anh/Chị<br> Đả tin tưởng và ủng hộ.<br> Đây là mã đơn hàng của bạn <font color='blue'> " + code + " </font>";
                            content += "<br> Thời gian đặt đơn là " + DateTime.Now + " xác nhận <br>";
                            content += "<br>Vui lòng xác nhận đơn hàng<br>";
                            content += "<a href=\"http://localhost:4200/Login\">Xác nhận đơn hàng</a>";
                            string _from = "0306191061@caothang.edu.vn";
                            string _subject = "Shop nhận đơn";
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
                            return Ok(new Response { Status = "Failed", Message = " Sales invoice generation failed" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response { Status = "Failed", Message = ex.Message });
                    }
                }                
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = " Cart null" });
            }
        }

        private bool BillOfSaleExists(int id)
        {
            return (db_.BillOfSale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
