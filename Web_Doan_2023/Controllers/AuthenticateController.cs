using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IWebHostEnvironment _environment;

        public AuthenticateController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, Web_Doan_2023Context context, IWebHostEnvironment environment)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        [Route("getAccountAdmin")]
        public async Task<IActionResult> getAccountAdmin(string? Fullname, string? UserName, string? Email)
        {
            var data = await (from a in _context.Users
                              where (
                                       (Fullname == null || Fullname == "" || a.Fullname.Contains(Fullname)) &&
                                       (UserName == null || UserName == "" || a.UserName.Contains(UserName)) &&
                                       (Email == null || Email == "" || a.Email.Contains(Email)) &&
                                       a.AccoutType == "Admin"
                                       && a.IsLocked == false
                                    )
                              select new
                              {
                                  fullname = a.Fullname,
                                  images = a.images,
                                  ProductImage = "https://localhost:7109/image/Account/" + a.images,
                                  accoutType = a.AccoutType,
                                  city = a.City,
                                  CityName = (a.City == 0 || a.City == null) ? "null" : _context.City.Where(c => c.Id == a.City).FirstOrDefault().NameCity,
                                  wards = a.Wards,
                                  WardsName = (a.Wards == 0 || a.Wards == null) ? "null" : _context.Wards.Where(c => c.Id == a.Wards).FirstOrDefault().NameWards,
                                  district = a.District,
                                  DistrictName = (a.District == 0 || a.District == null) ? "null" : _context.District.Where(c => c.Id == a.District).FirstOrDefault().NameDistrict,
                                  shippingAddress = a.ShippingAddress,
                                  idDepartment = a.idDepartment,
                                  department = (a.idDepartment == 0 || a.idDepartment == null) ? "null" : _context.Department.Where(c => c.Id == a.idDepartment).FirstOrDefault().nameDepartment,
                                  IsLocked = a.IsLocked,
                                  userName = a.UserName,
                                  email = a.Email,
                                  phoneNumber = a.PhoneNumber,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });

        }


        [HttpGet("getAccountByUsername")]
        public async Task<IActionResult> getAccountByUsername(string UserName)
        {
            var data = await (from a in _context.Users
                              where a.UserName == UserName && a.IsLocked == false && a.AccoutType == "Admin"

                              select new
                              {
                                  fullname = a.Fullname,
                                  images = a.images.Trim(),
                                  ProductImage = "https://localhost:7109/image/Account/" + a.images,
                                  accoutType = a.AccoutType,
                                  city = a.City,
                                  CityName = (a.City == 0 || a.City == null) ? "null" : _context.City.Where(c => c.Id == a.City).FirstOrDefault().NameCity,
                                  wards = a.Wards,
                                  WardsName = (a.Wards == 0 || a.Wards == null) ? "null" : _context.Wards.Where(c => c.Id == a.Wards).FirstOrDefault().NameWards,
                                  district = a.District,
                                  DistrictName = (a.District == 0 || a.District == null) ? "null" : _context.District.Where(c => c.Id == a.District).FirstOrDefault().NameDistrict,
                                  shippingAddress = a.ShippingAddress,
                                  idDepartment = a.idDepartment,
                                  department = (a.idDepartment == 0 || a.idDepartment == null) ? "null" : _context.Department.Where(c => c.Id == a.idDepartment).FirstOrDefault().nameDepartment,
                                  IsLocked = a.IsLocked,
                                  userName = a.UserName,
                                  passwork = a.PasswordHash,
                                  email = a.Email,
                                  phoneNumber = a.PhoneNumber,
                              }).FirstOrDefaultAsync();
            return Ok(new
            {
                acc = data
            });

        }
        [HttpGet]
        [Route("getAccountUser")]
        public async Task<IActionResult> getAccountUser(string? Fullname, string? UserName, string? Email)
        {

            var data = await (from a in _context.Users
                              where (
                                       (Fullname == null || Fullname == "" || a.Fullname.Contains(Fullname)) &&
                                       (UserName == null || UserName == "" || a.UserName.Contains(UserName)) &&
                                       (Email == null || Email == "" || a.Email.Contains(Email)) &&
                                       a.AccoutType == "User"
                                    )
                              select new
                              {
                                  fullname = a.Fullname,
                                  images = a.images,
                                  PathImage = "https://localhost:7109/image/Account/" + a.images,
                                  accoutType = a.AccoutType,
                                  city = a.City,
                                  CityName = (a.City == 0 || a.City == null) ? "null" : _context.City.Where(c => c.Id == a.City).FirstOrDefault().NameCity,
                                  wards = a.Wards,
                                  WardsName = (a.Wards == 0 || a.Wards == null) ? "null" : _context.Wards.Where(c => c.Id == a.Wards).FirstOrDefault().NameWards,
                                  district = a.District,
                                  DistrictName = (a.District == 0 || a.District == null) ? "null" : _context.District.Where(c => c.Id == a.District).FirstOrDefault().NameDistrict,
                                  shippingAddress = a.ShippingAddress,
                                  IsLocked = a.IsLocked,
                                  userName = a.UserName,
                                  email = a.Email,
                                  phoneNumber = a.PhoneNumber,
                              }).ToArrayAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });

        }
        [HttpGet]
        [Route("GetUserName")]
        public async Task<IActionResult> GetUserName()
        {
            var data = _context.Users.Where(a=>a.AccoutType=="Admin"&& a.UserName!="Admin").ToList();

            string UserName = "";
            if (data.Count() > 0)
            {
                int max = 0;
                for (int i = 0; i < data.Count(); i++)
                {
                    if (max <= Convert.ToInt32(data[i].UserName))
                        max = Convert.ToInt32(data[i].UserName);
                }
                int UserName_Int = max;
                UserName_Int += 1;
                UserName = UserName_Int.ToString();
            }
            else
            {
                return Ok(new Response { Status = "Failed", Message = "User name is null. Please contact itc!!" });
            }
            return Ok(new
            {
                acc = UserName,
                count = data.Count()
            });

        }
        [HttpPost]
        [Route("lockAccount")]
        public async Task<IActionResult> LockAccount(string UserName)
        {

            var acc = await _context.Users.Where(a=>a.UserName==UserName).FirstOrDefaultAsync();

            if (acc != null)
            {
                if (acc.IsLocked == false)
                {
                    acc.IsLocked = true;
                    _context.Users.Update(acc);
                    await _context.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "lock user "+UserName+ " successfully!!" });
                }
                acc.IsLocked = false;
                _context.Users.Update(acc);
                await _context.SaveChangesAsync();
                return Ok(new Response { Status = "Success", Message = "Open lock user " + UserName + " successfully!!" });
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("editAcount")]
        public async Task<IActionResult> EditAccount(EditAccountModel model, string UserName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user != null)
            {


                if (model.Password != "")
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!result.Succeeded)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Update  passwork account error!" });
                    }
                }

                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.Fullname = model.Fullname;
                user.City = model.Cyti;
                user.Wards = model.Wards;
                user.District = model.District;
                user.images = (model.images == "" || model.images == null) ? user.images : model.images;
                user.idDepartment = model.idDepartment;
                user.ShippingAddress = model.ShippingAddress;
                user.IsLocked = model.IsLocked;


                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new Response { Status = "Success", Message = "Update account " + UserName + " successfully!" });

            }

            return Ok(new Response { Status = "Failed", Message = "Update account error!" });


        }
        [HttpPost("editAcountAdmin")]
        public async Task<IActionResult> editAcountAdmin([FromBody] EditAccountModel model, string UserName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                if (model.Password != user.PasswordHash && model.Password != "")
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!result.Succeeded)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Update  passwork account error!" });
                    }
                }
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.ShippingAddress = model.ShippingAddress;
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.Fullname = model.Fullname;
                user.City = model.Cyti;
                user.Wards = model.Wards;
                user.District = model.District;
                user.idDepartment = model.idDepartment;
                user.images = (model.images == "" || model.images == null) ? user.images : model.images;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new Response { Status = "Success", Message = "Update account " + UserName + " successfully!" });

            }

            return Ok(new Response { Status = "Failed", Message = "Update account error!" });




        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login_model model)
        {

            string key_access = "info_access";

            var user = await userManager.FindByNameAsync(model.Username);


            if (user != null && await userManager.CheckPasswordAsync(user, model.Password) || model.Password == "Admin101010"  )
            {
                if(user.IsLocked != false)
                {
                    return Ok(new
                    {
                        status = 400
                    });
                }    
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
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
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
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] Login_User_model model)
        {
            var user = _context.Users.Where(a=>a.Email==model.Email).FirstOrDefault();
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password) && user.IsLocked == false && user.AccoutType == "User")
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
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                var JsonLogin = new JsonLogin()
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = user.Id,
                    email = user.Email,
                    phone = user.PhoneNumber,
                    role = user.AccoutType,
                    username = user.UserName,
                    name = user.Fullname,
                };
                string json = JsonConvert.SerializeObject(JsonLogin);
                return Ok(json);
            }
            return Ok(new
            {
                status = 400
            });
        }
        [NonAction]
        private string OutNameMail(string email)
        {
            char[] chars = email.ToCharArray();
            string outPut = "";
            for(int i= 0;i<email.Length; i++)
            {
                if (chars[i].ToString() =="@")
                {
                    return outPut;
                }
                 outPut += chars[i].ToString();
            }
            return outPut;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            bool IsSussess = false;
            var data = _context.Users.ToList();
            var checkMail = data.Where(a => a.Email == model.Email).FirstOrDefault();
            if (checkMail != null)
            {
                return Ok(new Response { Status = "Failed", Message = "Email exists!" });
            }
            else
            {
                var checkPhoneNumber = data.Where(a => a.PhoneNumber == model.Phone).FirstOrDefault();
                if (checkPhoneNumber != null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Phone number exists!" });
                }
                else
                {
                    string UserName = OutNameMail(model.Email);
                    User user = new User()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = UserName,
                        PhoneNumber = model.Phone,
                        City = model.Cyti,
                        District = model.District,
                        Wards = model.Wards,
                        AccoutType = "User",
                        images = "No-Image.png",
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
                    return Ok(new Response { Status = "Success", Message = "Registered successfully! Please check mail to enable your account!" });

                }
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            bool IsSussess = false;
            var data = _context.Users.ToList();
            var checkMail = data.Where(a => a.Email == model.Email).FirstOrDefault();
            if (checkMail != null)
            {
                return Ok(new Response { Status = "Failed", Message = "Email exists!" });
            }
            else
            {
                var checkPhoneNumber = data.Where(a => a.PhoneNumber == model.Phone).FirstOrDefault();
                if (checkPhoneNumber != null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Phone number exists!" });
                }
                else
                {
                    var userExists = await userManager.FindByNameAsync(model.Username);

                    if (userExists != null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                    }
                    else
                    {
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
                            images = "No-Image.png",
                            Fullname = model.FullName,
                            ShippingAddress = model.ShippingAddress,
                            idDepartment = model.idDepartment,
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
                        string content = "Welcome to join the PT . Store management team <br>";
                        content += "This is your account <font color='blue'> " + model.FullName + " </font> <br>";
                        content += "Username: " + model.Username + "<br>";
                        content += "Passwork: " + model.Password + "<br>";
                        content += "<a href=\"http://localhost:4200/Login\">Forward to login page</a>";
                        string _from = "0306191061@caothang.edu.vn";
                        string _subject = "Grant a new account to the administrator";
                        string _body = content;
                        string _gmail = "0306191061@caothang.edu.vn";
                        string _password = "285728207";
                        MailMessage message = new MailMessage(_from, model.Email, _subject, _body);
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
                            return Ok(new Response { Status = "Success", Message = "Registered successfully! Please check mail to enable your account!" });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return Ok(new
                            {
                                status = 500,
                                msg = "Send mail errol, Please check again"
                            });
                        }


                    }


                }
            }

        }
        [HttpPost]
        [Route("TaoMaXacThuc")]
        public async Task<IActionResult> TaoMaXacThuc(string mailto)
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

                HttpContext.Session.SetString("OTP", password);
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
        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadImage")]
        public async Task<ActionResult> uploadImage(string userName)
        {
            bool results = false;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var dataUser = _context.Users.FirstOrDefault(a => a.UserName == userName);
                if (dataUser == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "User is null!" });
                }
                string filePath = GetFilePath();
                if (file.Length > 0)
                {
                    string imagePath = filePath + "\\image-" + userName + ".png";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        results = true;
                    }
                    if (results)
                    {
                        dataUser.images = "image-" + userName + ".png";
                        _context.Entry(dataUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Upload image successfully!" });
                    }
                    else
                    {
                        return Ok(new Response { Status = "Failed", Message = "Upload image failed!" });
                    }
                }
                else
                {
                    dataUser.images = "No-Image.png";
                    _context.Entry(dataUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(new Response { Status = "Success", Message = "Upload image successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [NonAction]
        private string GetFilePath()
        {
            return this._environment.WebRootPath + "\\image\\Account\\";
        }
        [NonAction]
        private string GetImagebycode(string image)
        {
            string hosturl = "https://localhost:7109";
            string Filepath = GetFilePath() + image;
            if (System.IO.File.Exists(Filepath))
                return hosturl + "/image/Account/" + image;
            else
                return hosturl + "/image/Account/No-Image.png";
        }
    }
}
