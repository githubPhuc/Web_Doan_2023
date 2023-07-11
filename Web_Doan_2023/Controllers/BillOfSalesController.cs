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
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using System.Data;

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
        public async Task<ActionResult> GetList(string? code, string? Status)
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
            var data = list.Where(a=>(code==null||code==""||a.code.ToUpper().Contains(code.ToUpper()))&&
                                      Status == null || Status == ""||Status== "undefined" || a.StatusBill.Contains(Status)).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListDetail")]
        public async Task<ActionResult> GetListDetail(int  id)
        {
            var data = (from a in db_.BillOfSaleDetail
                        where a.IdBill == id
                        select new
                        {
                            Id = a.Id,
                           Quantity = a.Quantity,
                            Price = a.Price,
                            Idproduct = a.Idproduct,
                            nameProduct =(a.Idproduct==0?"":db_.Product.FirstOrDefault(c=>c.Id==a.Idproduct).nameProduct),
                            IdBill = a.IdBill,
                            Status = a.Status,

                        }).ToList();
            return Ok(new
            {
                data = data,
                count = data.Count()
            });
        }
        [HttpGet("SalesReport")]
        public async Task<ActionResult> SalesReport(string?start , string? End,string?Status)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("en-GB");
            var _NgayBatDau = new DateTime();
            var _NgayKetthuc = new DateTime();
            DateTime _NgayBD = new DateTime();
            DateTime _NgayKT = new DateTime();
            if (!string.IsNullOrEmpty(start))
            {
                try
                {
                    _NgayBatDau = DateTime.ParseExact(start, "dd/MM/yyyy", cul);

                    _NgayBD = new DateTime(_NgayBatDau.Year, _NgayBatDau.Month, _NgayBatDau.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(End))
            {
                try
                {
                    _NgayKetthuc = DateTime.ParseExact(End, "dd/MM/yyyy", cul);

                    _NgayKT = new DateTime(_NgayKetthuc.Year, _NgayKetthuc.Month, _NgayKetthuc.Day, 0, 0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
           

            var list = (from a in db_.BillOfSale
                        where Status == null || Status == "" || a.StatusBill.Contains(Status)
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
                            Status = a.Status,

                        }).ToList();

            if (_NgayBD!=null&&_NgayBatDau < _NgayKT&& _NgayKT!=null)
            {
                var data = list.Where(a => a.createDate >= _NgayBD && a.createDate <= _NgayKT).ToList();
                return Ok(new
                {
                    data = data,
                    count = data.Count()
                });
            }
            return Ok(new
            {
                data = list,
                count = list.Count()
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
        [HttpPost("CreateDetailBillOfSale")]
        public async Task<ActionResult> CreateDetailBillOfSale(string user,string Address,string phone,bool cod)
        {
            var dataUser = db_.Users.Where(a => a.UserName == user).FirstOrDefault();
            bool isSuccess = false;
            var dataCart = db_.CartProduct.Where(a => a.Username == user).ToList();
            if (dataCart.Count() > 0)
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
                        StatusCode = cod,
                        IsDelete = false,
                        Status = false,

                    };
                    if(cod==true)
                    {
                        dataBill.StatusBill = isStatus.Confirmed;
                    }
                    else
                    {
                        dataBill.StatusBill = isStatus.waitForConfirmation;
                    }
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
                                var checkDetailBill = db_.BillOfSaleDetail.Where(a => a.Idproduct == item.ProductId&& a.IdBill==dataBill.Id).FirstOrDefault();
                                if (checkDetailBill == null)
                                {
                                    var data = new BillOfSaleDetail()
                                    {
                                        IdBill = dataBill.Id,
                                        Idproduct = item.ProductId,
                                        Quantity = item.Quantity,
                                        Price = (item.salePrice == 0) ? item.Price : Convert.ToDecimal(item.salePrice),
                                        Status = true,
                                    };
                                    db_.BillOfSaleDetail.Add(data);
                                    await db_.SaveChangesAsync();
                                    dataBill.sumPrice += data.Price;
                                    dataBill.sumQuantity += data.Quantity;
                                    db_.Entry(dataBill).State = EntityState.Modified;
                                    await db_.SaveChangesAsync();
                                    isSuccess = true;
                                }
                                else
                                {
                                    checkDetailBill.Quantity += item.Quantity;
                                    db_.Entry(checkDetailBill).State = EntityState.Modified;
                                    await db_.SaveChangesAsync();

                                    dataBill.sumQuantity += checkDetailBill.Quantity;
                                    dataBill.sumPrice += checkDetailBill.Price;
                                    db_.Entry(dataBill).State = EntityState.Modified;
                                    await db_.SaveChangesAsync();
                                    isSuccess = true;
                                }
                                   
                            }
                        }
                    }
                    if(isSuccess== true)
                    {
                        string content = "ĐẶT ĐƠN HÀNG THÀNH CÔNG<br>";
                        content += "Kính gửi:Anh/Chị<br> Đả tin tưởng và ủng hộ.<br> Đây là mã đơn hàng của bạn <font color='blue'> " + code + " </font>";
                        content += "<br> Thời gian đặt đơn là " + DateTime.Now + " xác nhận <br>";
                        content += "<br>Trạng thái đơn hàng:"+ dataBill.StatusBill + "<br>";
                        content += "<a href=\"http://localhost:4200/Login\">Xác nhận đơn hàng</a>";
                        string _from = "0306191061@caothang.edu.vn";
                        string _subject = "TRẠNG THÁI ĐƠN HÀNG";
                        string _body = content;
                        string _gmail = "0306191061@caothang.edu.vn";
                        string _password = "285728207";
                        MailMessage message = new MailMessage(_from, dataUser.Email, _subject, _body);
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
                            db_.CartProduct.Where(a=>a.Username== user).ToList().ForEach(p => db_.CartProduct.Remove(p));
                            db_.SaveChanges();
                            return Ok(new Response { Status = "Success", Message = "Create bill successfully" });
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
            else
            {
                return Ok(new Response { Status = "Failed", Message = " Cart null" });
            }
        }
        [HttpPost("acceptance")]
        public async Task<ActionResult> acceptance(int id, string Username,string Status)
        {
            
            var data = db_.BillOfSale.Where(a => a.Id == id).FirstOrDefault();
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = " Invoice null" });
            }
            else
            {
                if (data.IsDelete == true)
                {
                    return Ok(new Response { Status = "Failed", Message = "The invoice has been deleted" });
                }
                else
                {

                    if (Status == isStatus.Confirmed
                        && data.StatusBill != isStatus.DeliverySuccessful
                        && data.StatusBill != isStatus.PackingGoods
                        && data.StatusBill != isStatus.SwitchToA_DedicatedTransportUnit
                        && data.StatusBill != isStatus.DeliveryInProgress)// đả xác nhận
                    {
                        data.UsernameUpdate = Username;
                        data.updateDate = DateTime.Now;
                        data.StatusBill = isStatus.Confirmed;
                    }
                    else if (Status == isStatus.PackingGoods
                        && data.StatusBill != isStatus.waitForConfirmation
                        && data.StatusBill != isStatus.DeliverySuccessful
                        && data.StatusBill != isStatus.SwitchToA_DedicatedTransportUnit
                        && data.StatusBill != isStatus.DeliveryInProgress)// đang gói hàng
                    {
                        data.UsernameUpdate = Username;
                        data.updateDate = DateTime.Now;
                        data.StatusBill = isStatus.PackingGoods;
                        var dataDetail = db_.BillOfSaleDetail.Where(a => a.IdBill == data.Id).ToList();
                        if (dataDetail.Count > 0)
                        {
                            foreach (var item in dataDetail)
                            {
                                var dataProduct = db_.Product.Where(a => a.Id == item.Idproduct).FirstOrDefault();
                                var dataProductDepot = db_.productDepot.Where(a => a.idProduct == item.Idproduct&&a.idDepot== dataProduct.IdDepot).FirstOrDefault();
                                dataProductDepot.QuantityProduct -= item.Quantity;
                                db_.Entry(dataProductDepot).State = EntityState.Modified;
                                await db_.SaveChangesAsync();
                            }
                        }
                        else
                        {

                            return Ok(new Response { Status = "Failed", Message = "Invoice details not found" });
                        }
                    }
                    else if (Status == isStatus.SwitchToA_DedicatedTransportUnit
                        && data.StatusBill != isStatus.waitForConfirmation
                        && data.StatusBill != isStatus.Confirmed
                        && data.StatusBill != isStatus.SwitchToA_DedicatedTransportUnit
                        && data.StatusBill != isStatus.DeliveryInProgress)// giao đươn vị vận chuyển
                    {
                        data.UsernameUpdate = Username;
                        data.updateDate = DateTime.Now;
                        data.StatusBill = isStatus.SwitchToA_DedicatedTransportUnit;
                    }
                    else if (Status == isStatus.DeliveryInProgress
                        && data.StatusBill != isStatus.waitForConfirmation
                        && data.StatusBill != isStatus.Confirmed
                        && data.StatusBill != isStatus.PackingGoods
                        && data.StatusBill != isStatus.DeliveryInProgress)//đang giao hàng
                    {
                        data.UsernameUpdate = Username;
                        data.updateDate = DateTime.Now;
                        data.StatusBill = isStatus.DeliveryInProgress;
                    }
                    else if (Status == isStatus.DeliveryInProgress
                        && data.StatusBill != isStatus.waitForConfirmation
                        && data.StatusBill != isStatus.Confirmed
                        && data.StatusBill != isStatus.PackingGoods
                        && data.StatusBill != isStatus.SwitchToA_DedicatedTransportUnit)// giao hàng thành công
                    {
                        data.UsernameUpdate = Username;
                        data.updateDate = DateTime.Now;
                        data.StatusBill = isStatus.DeliverySuccessful;
                        data.Status = true;
                        data.StatusCode = true;
                    }
                    else
                    {
                        return Ok(new Response { Status = "Failed", Message = "Status Error :"+ Status });
                    }
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    var dataUser = db_.Users.FirstOrDefault(a => a.UserName == data.UsernameCreate);
                    string content = "Đơn hàng đả được chuyển đến bước tiếp theo<br>";
                    content += "Kính gửi:Anh/Chị<br> Đả tin tưởng và ủng hộ.<br> Mã đơn hàng của bạn <font color='red'> " + data.code + " </font> đã được phê duyệt";
                    content += "<br> Thời gian đặt đơn là " + data.createDate + " <br>";
                    content += "<br> Thời gian đơn được duyệt là " + DateTime.Now + "  <br>";
                    content += "<br> Số lượng sản phẩm là " + data.sumQuantity + " món <br>";
                    content += "<br> Tổng tiền là " + data.sumPrice + " VND <br>";
                    content += "<br> Trạng thái đơn hàng là <font color='blue'>" + data.StatusBill + " </font> <br>";
                    content += "<a href=\"http://localhost:4200/Login\">Kiểm tra đơn hàng</a>";
                    string _from = "0306191061@caothang.edu.vn";
                    string _subject = "TRẠNG THÁI ĐƠN HÀNG";
                    string _body = content;
                    string _gmail = "0306191061@caothang.edu.vn";
                    string _password = "285728207";
                    MailMessage message = new MailMessage(_from, dataUser!.Email, _subject, _body);
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
                        return Ok(new Response { Status = "Success", Message = "Send mail successfully" });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return Ok(new Response { Status = "Failed", Message = "Send mail Failed" });
                    }

                }
            }
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(int id,string Username)
        {
            var dataUser = db_.Users.FirstOrDefault(a => a.UserName == Username);
            var data = db_.BillOfSale.Where(a => a.Id == id).FirstOrDefault();
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = " Invoice null" });
            }
            else
            {
                if (data.IsDelete == true)
                {
                    return Ok(new Response { Status = "Failed", Message = "The invoice has been deleted" });
                }
                if (data.StatusBill != isStatus.waitForConfirmation)
                {
                    return Ok(new Response { Status = "Failed", Message = "Order has been " +data.StatusBill});
                }
                else
                {
                    var dataDetail = db_.BillOfSaleDetail.Where(a => a.IdBill == data.Id).ToList();
                    if (dataDetail.Count > 0)
                    {
                        foreach (var item in dataDetail)
                        {
                            var dataProductDepot = db_.productDepot.Where(a => a.idProduct == item.Idproduct).FirstOrDefault();
                            dataProductDepot.QuantityProduct += item.Quantity;
                            db_.Entry(dataProductDepot).State = EntityState.Modified;
                            await db_.SaveChangesAsync();
                        }
                    }
                    data.IsDelete = true;
                    data.UsernameDelete = Username;
                    data.updateDate = DateTime.Now;
                    data.StatusBill = isStatus.Cancel;
                    db_.Entry(data).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
                    string content = "mail Xác nhận đơn hàng<br>";
                    content += "Kính gửi:Anh/Chị<br> Đả tin tưởng và ủng hộ.<br> Mã đơn hàng của bạn <font color='blue'> " + data.code + " </font> Đã được hũy";
                    content += "<br> Thời gian đặt đơn là " + data.createDate + " <br>";
                    content += "<br> Thời gian đơn bị hủy là " + DateTime.Now + "  <br>";
                    content += "<a href=\"http://localhost:4200/Login\">Kiểm tra đơn hàng</a>";
                    string _from = "0306191061@caothang.edu.vn";
                    string _subject = "Shop xác nhận đơn";
                    string _body = content;
                    string _gmail = "0306191061@caothang.edu.vn";
                    string _password = "285728207";
                    MailMessage message = new MailMessage(_from, dataUser!.Email, _subject, _body);
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
                        return Ok(new Response { Status = "Success", Message = "Send mail successfully" });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return Ok(new Response { Status = "Failed", Message = "Send mail Failed" });
                    }
                }
            }
        }
        
        private bool BillOfSaleExists(int id)
        {
            return (db_.BillOfSale?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
