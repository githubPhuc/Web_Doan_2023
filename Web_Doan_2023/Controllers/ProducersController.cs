using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Data;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;
        private readonly IWebHostEnvironment _environment;
        public ProducersController(Web_Doan_2023Context context, IWebHostEnvironment environment)
        {
            db_ = context;
            _environment = environment;
        }

        // GET: api/Producers
        [HttpGet("GetList")]
        public async Task<ActionResult> GetList(string? name, string? code)
        {
            var data = await (from a in db_.Producer
                              where (name == null || name == "" || a.nameProduce.ToUpper().Contains(name.ToUpper())) &&
                                    (code == null || code == "" || a.codeProduce.ToUpper().Contains(code.ToUpper()))
                              select new
                              {
                                  id = a.Id,
                                  nameProduce = a.nameProduce,
                                  codeProduce = a.codeProduce,
                                  Email = a.Email,
                                  Phone = a.Phone,
                                  city = a.idCity,
                                  CityName = (a.idCity == 0 || a.idCity == null) ? "null" : db_.City.Where(c => c.Id == a.idCity).FirstOrDefault().NameCity,
                                  wards = a.idWards,
                                  WardsName = (a.idWards == 0 || a.idWards == null) ? "null" : db_.Wards.Where(c => c.Id == a.idWards).FirstOrDefault().NameWards,
                                  district = a.idDistrict,
                                  DistrictName = (a.idDistrict == 0 || a.idDistrict == null) ? "null" : db_.District.Where(c => c.Id == a.idDistrict).FirstOrDefault().NameDistrict,
                                  Location = a.Location,
                                  Status = a.Status,
                                  logoProducer = a.logoProducer,
                                  pathLogo = "https://localhost:7109/image/Producer/" + a.logoProducer,
                              }).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpGet("GetListById")]
        public async Task<ActionResult> GetListById(int id)
        {
            var data = await (from a in db_.Producer
                              where a.Id == id
                              select new
                              {
                                  id = a.Id,
                                  nameProduce = a.nameProduce,
                                  codeProduce = a.codeProduce,
                                  Email = a.Email,
                                  Phone = a.Phone,
                                  city = a.idCity,
                                  CityName = (a.idCity == 0 || a.idCity == null) ? "null" : db_.City.Where(c => c.Id == a.idCity).FirstOrDefault().NameCity,
                                  wards = a.idWards,
                                  WardsName = (a.idWards == 0 || a.idWards == null) ? "null" : db_.Wards.Where(c => c.Id == a.idWards).FirstOrDefault().NameWards,
                                  district = a.idDistrict,
                                  DistrictName = (a.idDistrict == 0 || a.idDistrict == null) ? "null" : db_.District.Where(c => c.Id == a.idDistrict).FirstOrDefault().NameDistrict,
                                  Location = a.Location,
                                  Status = a.Status,
                                  logoProducer = a.logoProducer,
                                  pathLogo = "https://localhost:7109/image/Producer/" + a.logoProducer,
                              }).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpGet("Load_CBX")]
        public async Task<ActionResult> Load_CBX()
        {
            var data = await (from a in db_.Producer
                              select new
                              {
                                  id = a.Id,
                                  nameProduce = a.nameProduce,
                                  codeProduce = a.codeProduce,
                              }).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromForm] Producer model, int id)
        {
            if (model == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Data is null!" });
            }
            else
            {

                try
                {

                    var data = await db_.Producer.Where(a => a.Id == id).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Producer not exist!" });
                    }
                    else
                    {
                        data.Phone = model.Phone;
                        data.codeProduce = model.codeProduce;
                        data.Email = model.Email;
                        data.idCity = model.idCity;
                        data.idWards = model.idWards;
                        data.idDistrict = model.idDistrict;
                        data.logoProducer = "No-Image.png";
                        data.Status = model.Status;
                        data.Location = model.Location;

                        db_.Entry(data).State = EntityState.Modified;
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Update producer " + model.nameProduce + " successfully!" });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }

        }
        [HttpPost("StopCooperating")]
        public async Task<ActionResult> StopCooperating(int id)// Ngừng hợp tác 
        {
            if (id == null|| id<=0)
            {
                return Ok(new Response { Status = "Failed", Message = "Id is null!" });
            }
            else
            {

                try
                {

                    var data = await db_.Producer.Where(a => a.Id == id).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Producer is not exist!" });
                    }
                    else
                    {
                        if(data.Status==true)
                        {
                            data.Status = false;
                        }
                        else
                        {
                            data.Status = true;
                        }
                        db_.Entry(data).State = EntityState.Modified;
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Stop Cooperating producer " + data.nameProduce + " successfully!" });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }

        }

        [HttpPost("Insert")]
        public async Task<ActionResult> Insert([FromBody] Producer model)
        {
            if (model == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Data is null!" });
            }
            else
            {
                try
                {
                    var check = db_.Producer.Where(a => a.codeProduce == model.codeProduce && a.Phone == model.Phone).ToList();
                    if (check.Count() > 0)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Code producer is not exist!" });
                    }
                    else
                    {
                        var data = new Producer()
                        {
                            Phone = model.Phone,
                            codeProduce = model.codeProduce,
                            Email = model.Email,
                            idCity = model.idCity,
                            idWards = model.idWards,
                            idDistrict = model.idDistrict,
                            logoProducer = "No-Image.png",
                            Status = model.Status,
                            Location = model.Location,
                        };
                        db_.Producer.Add(data);
                        db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Insert producer " + model.nameProduce + " successfully!" });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }

        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Id is null!" });
            }
            else
            {
                try
                {
                    var data = await db_.Producer.Where(a => a.Id==id).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        return Ok(new Response { Status = "Failed", Message = "Producer is not exist!" });
                    }
                    else
                    {
                        if(data.Status== true)
                        {
                            return Ok(new Response { Status = "Failed", Message = "Stop cooperating before deleting. Please check again!" });
                        }
                        db_.Producer.Remove(data);
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Delete producer " + data.nameProduce + " successfully!" });
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = "Failed", Message = ex.Message });
                }
            }

        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadImage")]
        public async Task<ActionResult> uploadImage(string code)
        {
            bool results = false;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var dataProduce = db_.Producer.FirstOrDefault(a => a.codeProduce == code);
                if (dataProduce == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Producer is null!" });
                }
                string filePath = GetFilePath();
                if (file.Length > 0)
                {
                    string imagePath = filePath + "\\image-" + dataProduce.codeProduce + ".png";
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
                        dataProduce.logoProducer = "image-" + dataProduce.codeProduce + ".png";
                        db_.Entry(dataProduce).State = EntityState.Modified;
                        await db_.SaveChangesAsync();
                        return Ok(new Response { Status = "Success", Message = "Upload image successfully!" });
                    }
                    else
                    {
                        return Ok(new Response { Status = "Failed", Message = "Upload image failed!" });
                    }
                }
                else
                {
                    dataProduce.logoProducer = "No-Image.png";
                    db_.Entry(dataProduce).State = EntityState.Modified;
                    await db_.SaveChangesAsync();
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
            return this._environment.WebRootPath + "\\image\\Producer\\";
        }
        [NonAction]
        private string GetImagebycode(string image)
        {
            string hosturl = "https://localhost:7109";
            string Filepath = GetFilePath() + image;
            if (System.IO.File.Exists(Filepath))
                return hosturl + "/image/Producer/" + image;
            else
                return hosturl + "/image/Producer/No-Image.png";
        }

        private bool ProducerExists(int id)
        {
            return (db_.Producer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
