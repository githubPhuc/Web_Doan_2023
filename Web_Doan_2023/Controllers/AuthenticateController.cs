using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;
using Web_Doan_2023.Settings;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly Web_Doan_2023Context _context;


        public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, Web_Doan_2023Context context)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

        }

        [HttpGet]
        [Route("getAccount")]

        public async Task<IActionResult> GetAllAccount()
        {

            var result = (from a in _context.Users
                          select new
                          {
                              Id = a.Id,

                              Username = a.UserName,
                              Email = a.Email,
                              Phone = a.PhoneNumber,
                              Address = a.ShippingAddress,
                              AccoutType = a.AccoutType,
                              IsLocked = a.IsLocked,
                          }).ToList();
            var acc = await _context.Users.ToListAsync();

            return Ok(new
            {
                acc = result,
                count = acc.Count()
            });

        }
        [HttpGet]
        [Route("filAccount")]

        public async Task<IActionResult> FilterAccount(int id)
        {

            var result = (from a in _context.Users
                          where a.AccoutType == (id == 1 ? "Admin" : "User")
                          select new
                          {
                              Id = a.Id,

                              Username = a.UserName,
                              Email = a.Email,
                              Phone = a.PhoneNumber,
                              Address = a.ShippingAddress,
                              AccoutType = a.AccoutType,
                              IsLocked = a.IsLocked,
                          }).ToList();
            var acc = await _context.Users.ToListAsync();

            return Ok(new
            {
                acc = result,
                count = acc.Count()
            });

        }

        //[Route("searchAccount")]

        //public async Task<IActionResult> searchAccount(string txt)
        //{

        //    var result = (from a in _context.Users
        //                  where a.UserName.Contains(txt) || a.Email.Contains(txt) || a.PhoneNumber.Contains(txt)
        //                  select new
        //                  {
        //                      Id = a.Id,

        //                      Username = a.UserName,
        //                      Email = a.Email,
        //                      Phone = a.PhoneNumber,
        //                      Address = a.ShippingAddress,
        //                      AccoutType = a.AccoutType,
        //                      IsLocked = a.IsLocked,
        //                  }).ToList();
        //    var acc = await _context.Users.ToListAsync();

        //    return Ok(new
        //    {
        //        acc = result,
        //        count = acc.Count()
        //    });

        //}
        #region
        //[HttpGet]
        //[Route("getAccountById")]

        //public async Task<IActionResult> GetAccount(string id)
        //{

        //    var result = (from a in _context.Users
        //                  where a.Id == id
        //                  select new
        //                  {
        //                      Id = a.Id,
        //                      Fullname = a.Fullname,
        //                      Username = a.UserName,
        //                      Email = a.Email,
        //                      Phone = a.PhoneNumber,
        //                      Address = a.ShippingAddress,
        //                      AccoutType = a.AccoutType,
        //                      IsLocked = a.IsLocked,
        //                      Password = a.PasswordHash,
        //                      Count = (from b in _context.Invoice
        //                               where b.UserId == id
        //                               select b.Id).Count()
        //                  }).FirstOrDefault();
        //    return Ok(result);
        //}
        #endregion
        [HttpPost]
        [Route("lockAccount")]

        public async Task<IActionResult> LockAccount(string id)
        {

            var acc = await _context.Users.FindAsync(id);

            if (acc != null)
            {
                if (acc.IsLocked == true)
                {
                    acc.IsLocked = false;
                    _context.Users.Update(acc);
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        status = 200,
                        msg = "Đã mở khoá tài khoản"
                    });
                }
                acc.IsLocked = true;
                _context.Users.Update(acc);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    msg = "Đã khoá tài khoản"
                });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("editAcount")]

        public async Task<IActionResult> EditAccount(EditAccountModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {


                if (model.Password != "")
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!result.Succeeded)
                    {
                        return Ok(new
                        {
                            status = 500,
                            msg = "Cập nhật tài khoản thất bại"
                        });
                    }
                }

                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.ShippingAddress = model.Address;
                _context.Update(user);
                _context.SaveChanges();

                return Ok(new
                {
                    status = 200,
                    msg = "Đã cập nhật thông tin tài khoản"
                });

            }
            return Ok(new
            {
                status = 500,
                msg = "Cập nhật tài khoản thất bại"
            });


        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login_model model)
        {
                    
            string key_access = "info_access";

            var user = await userManager.FindByNameAsync(model.Username);


            if (user != null && await userManager.CheckPasswordAsync(user, model.Password) && user.IsLocked == false)
            {
                var User_role = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

                if (User_role.Count() > 0)
                {
                    foreach (var userRole in User_role)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                }
                else
                {

                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,



                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                HttpContext.Session.SetString("userName",user.UserName);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    address = user.ShippingAddress,
                    phone = user.PhoneNumber,
                    role = user.AccoutType,
                    username = user.UserName,
                    name = user.Fullname



                });
            }
            return Ok(new
            {
                status = 400
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);            
            if (userExists != null)
                return Ok(new
                {
                    Status = 500,
                    msg = "Tên tài khoản đã tồn tại !"
                });
            var us = await _context.Users.Where(u => u.Email == model.Email).ToListAsync();
            if (us.Count() > 0)
            {
                return Ok(new
                {
                    status = 500,
                    msg = "Email đã tồn tại !"
                });
            }
            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.Phone,
                City = model.Cyti,
                District =model.District,
                Wards=model.Wards,
                AccoutType = "User",
                Fullname = model.FullName,
                ShippingAddress = model.ShippingAddress,
                IsLocked = false

            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            var us = await _context.Users.Where(u => u.Email == model.Email).ToListAsync();
            if (us.Count() > 0)
            {
                return Ok(new
                {
                    status = 500,
                    msg = "Email already exits"
                });
            }
            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
               
                PhoneNumber = model.Phone,
                City = model.Cyti,
                District = model.District,
                Wards = model.Wards,
                AccoutType = "Admin",
                Fullname = model.FullName,
                ShippingAddress = model.ShippingAddress,
                IsLocked = false
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(User_role.Admin))
                await roleManager.CreateAsync(new IdentityRole(User_role.Admin));
            if (!await roleManager.RoleExistsAsync(User_role.User))
                await roleManager.CreateAsync(new IdentityRole(User_role.User));

            if (await roleManager.RoleExistsAsync(User_role.Admin))
            {
                await userManager.AddToRoleAsync(user, User_role.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }



        [HttpPost]
        [Route("TaoMaXacThuc")]

        public async Task<IActionResult> TaoMaXacThuc(   string mailto)
        {
            var email = await _context.Users.Where(t => t.Email == mailto).ToListAsync();
            if (email.Count > 0)//test
            {
                string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
                string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
                string Digits = "1234567890";
                string allCharacters = UpperCase + Digits;
                Random r = new Random();
                String password = "";
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
                //var pass64= System.Convert.ToBase64String(plainTextBytes);
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


                string content = "Đây là mã xác thực tài khoản <font color='blue'> Shop </font> của bạn <br>";
                string token = content + "<h1>" + password + "</h1>";
                string _from = "0306191061@caothang.edu.vn";
                string _subject = "Xác thực tài khoản 2PShop";
                string _body = token;
                string _gmail = "0306191061@caothang.edu.vn";
                string _password = "285728207";
                MailMessage message = new MailMessage(_from, mailto, _subject, _body);
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
                    return Ok(new
                    {
                        status = 200,
                        msg = "Mã xác thực đã gửi đến mail ",
                        otp = password
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Ok(new
                    {
                        status = 500,
                        msg = "Gửi thất bại, kiểm tra lại địa chỉ email"
                    });
                }

            }
            return Ok(new
            {
                status = 500,
                msg = "Email không đúng"
            });

        }

        [HttpPost]
        [Route("XacThuc")]

        public async Task<IActionResult> XacThuc(string otp)
        {
            if (HttpContext.Session.GetString("OTP") == otp)
            {
                return Ok(new
                {
                    status = 200,
                    msg = "Xác minh thành công"
                });
            }
            return Ok(new
            {
                status = 500,
                msg = "Xác mminh không thành công"
            });
        }

        [HttpPost]
        [Route("ChangePassword")]


        public async Task<IActionResult> ChangePassword(string mail, string newPass)
        {
            var user = await userManager.FindByEmailAsync(mail);

            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var result = await userManager.ResetPasswordAsync(user, token, newPass);

                return Ok(new
                {
                    status = 200,
                    msg = "Đã cập nhật password"
                });
            }
            return BadRequest();
        }

        //[HttpPost]
        // [Route("authMail")]

        //public async Task<IActionResult> AuthMail(string mail)
        //{

        //    var email = await _context.Users.Where(t => t.Email == mail).ToListAsync();
        //    if (email.Count == 0)
        //    {
        //        string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
        //        string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
        //        string Digits = "1234567890";
        //        string allCharacters = UpperCase + Digits;
        //        Random r = new Random();
        //        String password = "";
        //        //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
        //        //var pass64= System.Convert.ToBase64String(plainTextBytes);
        //        for (int i = 0; i < 6; i++)
        //        {
        //            double rand = r.NextDouble();
        //            if (i == 0)
        //            {
        //                password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
        //            }
        //            else
        //            {
        //                password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
        //            }
        //        }
        //        string content = "Đây là mã xác thực đăng ký tài khoản 2P Shop của bạn ! <br>";
        //        string token = content + "<h1>" + password + "</h1>";
        //        string _from = "0306191060@caothang.edu.vn";
        //        string _subject = "Xác thực mail";
        //        string _body = token;
        //        string _gmail = "0306191060@caothang.edu.vn";
        //        string _password = "301765153";
        //        MailMessage message = new MailMessage(_from, mail, _subject, _body);
        //        message.BodyEncoding = System.Text.Encoding.UTF8;
        //        message.SubjectEncoding = System.Text.Encoding.UTF8;
        //        message.IsBodyHtml = true;
        //        message.ReplyToList.Add(new MailAddress(_from));
        //        message.Sender = new MailAddress(_from);

        //        using var smtpClient = new SmtpClient("smtp.gmail.com");
        //        smtpClient.Port = 587;
        //        smtpClient.EnableSsl = true;
        //        smtpClient.Credentials = new NetworkCredential(_gmail, _password);

        //        try
        //        {
        //            await smtpClient.SendMailAsync(message);
        //            //HttpContext.Session.SetString("OTP", _body);
        //            //HttpContext.Session.SetString("Gmail", _to);
        //            //HttpContext.Session.SetString("Sdt", sdt);
        //            return Ok(new
        //            {
        //                status = 200,
        //                msg = "Mã xác thực đã gửi đến mail ",
        //                otp = password
        //            });
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //            return Ok(new
        //            {
        //                status = 500,
        //                msg = "Gửi thất bại, kiểm tra lại địa chỉ email"
        //            });
        //        }

        //    }
        //    return Ok(new
        //    {
        //        status = 500,
        //        msg = "Email không đúng"
        //    });
        //}//email
    }
}
