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
    public class ProducersController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public ProducersController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/Producers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producer>>> GetProducer()
        {
          if (_context.Producer == null)
          {
              return NotFound();
          }
            return await _context.Producer.ToListAsync();
        }

        // GET: api/Producers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producer>> GetProducer(int id)
        {
          if (_context.Producer == null)
          {
              return NotFound();
          }
            var producer = await _context.Producer.FindAsync(id);

            if (producer == null)
            {
                return NotFound();
            }

            return producer;
        }

        // PUT: api/Producers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducer(int id, Producer producer)
        {
            if (id != producer.Id)
            {
                return BadRequest();
            }

            _context.Entry(producer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Producers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producer>> PostProducer(Producer producer)
        {
          if (_context.Producer == null)
          {
              return Problem("Entity set 'Web_Doan_2023Context.Producer'  is null.");
          }
            _context.Producer.Add(producer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducer", new { id = producer.Id }, producer);
        }

        // DELETE: api/Producers/5
        [HttpPost]
        public async Task<IActionResult> DeleteProducer(int id)
        {
            if (_context.Producer == null)
            {
                return Ok(new Response { Status = "500", Message = "The Producer exists in the database!Unable to delete Producer!" });
            }
            var dataProduce = _context.Producer.FirstOrDefault(a=>a.Id==id);
            dataProduce.Status = false;
            _context.Entry(dataProduce).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                status = 200,
                msg = "Delete producer"+ dataProduce.nameProduce + " Success",
            });

        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProducer(int id)
        //{
        //    if (_context.Producer == null)
        //    {
        //        return NotFound();
        //    }
        //    var producer = await _context.Producer.FindAsync(id);
        //    if (producer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Producer.Remove(producer);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool ProducerExists(int id)
        {
            return (_context.Producer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
