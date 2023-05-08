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
    public class ImportBillDepotsController : ControllerBase
    {
        private readonly Web_Doan_2023Context _context;

        public ImportBillDepotsController(Web_Doan_2023Context context)
        {
            _context = context;
        }

        // GET: api/ImportBillDepots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportBillDepot>>> GetImportBillDepot()
        {
          if (_context.ImportBillDepot == null)
          {
              return NotFound();
          }
            return await _context.ImportBillDepot.ToListAsync();
        }

        // GET: api/ImportBillDepots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportBillDepot>> GetImportBillDepot(int id)
        {
          if (_context.ImportBillDepot == null)
          {
              return NotFound();
          }
            var importBillDepot = await _context.ImportBillDepot.FindAsync(id);

            if (importBillDepot == null)
            {
                return NotFound();
            }

            return importBillDepot;
        }

        // PUT: api/ImportBillDepots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportBillDepot(int id, ImportBillDepot importBillDepot)
        {
            if (id != importBillDepot.Id)
            {
                return BadRequest();
            }

            _context.Entry(importBillDepot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportBillDepotExists(id))
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

        // POST: api/ImportBillDepots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostImportBillDepot(ImportBillDepot importBillDepot)
        {
              if (_context.ImportBillDepot == null)
              {
                  return Problem("Entity set 'Web_Doan_2023Context.ImportBillDepot'  is null.");
              }
            var dataPillImport = new ImportBillDepot();
            dataPillImport.Id = importBillDepot.Id;
            dataPillImport.codeBill = importBillDepot.codeBill;
            dataPillImport.UpdatedDate = DateTime.Now;
            dataPillImport.CreatedDate = DateTime.Now;
            dataPillImport.Price   = importBillDepot.Price;
            dataPillImport.IsAcceptance = importBillDepot.IsAcceptance;
            dataPillImport.UserCreate = importBillDepot.UserCreate;
            dataPillImport.Status = isStatus.WaitingForApproval;
            _context.ImportBillDepot.Add(importBillDepot);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = 200,
                id = importBillDepot.Id,
            });
        }

        // DELETE: api/ImportBillDepots/5
        [HttpPost]
        public async Task<IActionResult> DeleteProducer(int id)
        {
            var dataImportBillDepots = _context.ImportBillDepot.FirstOrDefault(a => a.Id == id);
            if (dataImportBillDepots == null)
            {
                return Ok(new Response { Status = "500", Message = "The Producer exists in the database!Unable to delete Producer!" });
            }// kiểm tra 
            dataImportBillDepots.Status = isStatus.Cancel;
            _context.Entry(dataImportBillDepots).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                status = 200,
                msg = "Delete producer" + dataImportBillDepots.codeBill + " Success",
            });
        }// chưa kiểm tra

        private bool ImportBillDepotExists(int id)
        {
            return (_context.ImportBillDepot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
