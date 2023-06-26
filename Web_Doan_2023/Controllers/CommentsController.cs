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
    public class CommentsController : ControllerBase
    {
        private readonly Web_Doan_2023Context db_;

        public CommentsController(Web_Doan_2023Context context)
        {
            db_ = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult> GetComment(int idProduct,string User)
        {
            var data = await (from a in db_.Comment
                        join b in db_.CommentProduct on a.id equals b.IdComment
                        where b.idProduct == idProduct && b.Username == User
                        orderby a.BottomLevel ascending
                        select new
                        {
                            Content = a.Content,
                            Top = a.TopLevel,
                            Bottom = a.BottomLevel,
                            idProduct = idProduct,
                            User = User,
                            Date = a.Created,
                        }

                        ).ToListAsync();
            return Ok(new
            {
                acc = data,
                count = data.Count()
            });
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UpdateComment")]
        public async Task<ActionResult<Comment>> UpdateComment(string content, int id)
        {
            var data = db_.Comment.Where(a => a.id == id).FirstOrDefault();
            data.Content = content;
            data.Updated=DateTime.Now;
            data.Created=DateTime.Now;
            db_.Entry(data).State = EntityState.Modified;
            db_.SaveChanges();
            return Ok(new Response { Status = "Success", Message = "Comment successfully!" });
        }
        [HttpPost("PostComment")]
        public async Task<ActionResult> PostComment([FromBody] Comment comment, int idProduct, string username)
        {
            int id = 0;
            if (comment.TopLevel == 0)
            {
                var data = new Comment()
                {
                    Content = comment.Content,
                    TopLevel = comment.TopLevel,
                    BottomLevel = comment.BottomLevel,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                };
                db_.Comment.Add(data);
                db_.SaveChanges();
                id = data.id;
            }
            else
            {
                var checkComment = db_.Comment.Where(a => a.id == comment.TopLevel).FirstOrDefault();
                if (checkComment == null)
                {
                    return Ok(new Response { Status = "Failed", Message = "Id Comment level is null!" });
                }
                else
                {
                    var data = new Comment()
                    {
                        Content = comment.Content,
                        TopLevel = comment.TopLevel,
                        BottomLevel = comment.BottomLevel,
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                    };
                    db_.Comment.Add(data);
                    db_.SaveChanges();
                    id = data.id;
                }

            }
            var data_CommenProduct = new CommentProduct()
            {
                IdComment = id,
                idProduct = idProduct,
                Username = username,
            };
            db_.CommentProduct.Add(data_CommenProduct);
            db_.SaveChanges();
            return Ok(new Response { Status = "Success", Message = "Comment successfully!" });
        }

        // DELETE: api/Comments/5
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteComment(int id,int idproduct,string Username)
        {
            var data = db_.Comment.Where(a => a.id == id).FirstOrDefault();
            if (data == null)
            {
                return Ok(new Response { Status = "Failed", Message = "Comment is null!" });
            }

            else
            {
                var check = db_.CommentProduct.Where(a => a.idProduct == idproduct&& a.Username==Username&&a.IdComment==id).FirstOrDefault();
                if(check != null)
                {
                    db_.CommentProduct.Remove(check);
                    db_.Comment.Remove(data);
                    db_.SaveChanges();
                    return Ok(new Response { Status = "Success", Message = "Delete comment successfully!" });
                }
                else
                {
                    return Ok(new Response { Status = "Failed", Message = "Delete comment failed!" });
                }
            }
            
        }

        private bool CommentExists(int id)
        {
            return (db_.Comment?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
