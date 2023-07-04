using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestorauntAPI.Data;
using RestorauntAPI.Data.Models;

namespace RestorauntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerRoot
    {
        private readonly RestorauntDBContext _context;

        public TablesController(RestorauntDBContext context)
        {
            _context = context;
        }

        // GET: api/Tables
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Table>>> GetTables()
        {
          if (_context.Tables == null)
          {
              return NotFound();
          }
            return await _context.Tables.ToListAsync();
        }

        [HttpGet("free")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<Table>>> GetFreeTables(string date, string timeFrom, string timeTo)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeFrom) || string.IsNullOrEmpty(timeTo))
            {
                return BadRequest("date, timeFrom, and timeTo are required.");
            }

            if (!DateTime.TryParse(date, out DateTime bookingDate))
            {
                return BadRequest("Invalid date format.");
            }

            TimeSpan from;
            TimeSpan to;

            if (!TimeSpan.TryParse(timeFrom, out from))
            {
                return BadRequest("Invalid timeFrom format.");
            }

            if (!TimeSpan.TryParse(timeTo, out to))
            {
                return BadRequest("Invalid timeTo format.");
            }

            var bookingDateWithoutTime = bookingDate.Date;
            var bookings = await _context.Bookings
                .Where(b => b.Date.Date == bookingDateWithoutTime && !(from >= b.TimeTo || to <= b.TimeFrom))
                .ToListAsync();

            
            var tables = await _context.Tables.ToListAsync();
            var freeTables = tables.Where(t => !bookings.Any(b => b.TableID == t.ID)).ToList();

            return freeTables;
        }

        [HttpGet("free/sorted/desc")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<Table>>> GetFreeTablesSortedByCapacityDescending(string date, string timeFrom, string timeTo)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeFrom) || string.IsNullOrEmpty(timeTo))
            {
                return BadRequest("date, timeFrom, and timeTo are required.");
            }

            if (!DateTime.TryParse(date, out DateTime bookingDate))
            {
                return BadRequest("Invalid date format.");
            }

            TimeSpan from;
            TimeSpan to;

            if (!TimeSpan.TryParse(timeFrom, out from))
            {
                return BadRequest("Invalid timeFrom format.");
            }

            if (!TimeSpan.TryParse(timeTo, out to))
            {
                return BadRequest("Invalid timeTo format.");
            }

           
            var bookingDateWithoutTime = bookingDate.Date;

            var bookings = await _context.Bookings
                .Where(b => b.Date.Date == bookingDateWithoutTime && !(from >= b.TimeTo || to <= b.TimeFrom))
                .ToListAsync();


            var tables = await _context.Tables.ToListAsync();
            var freeTables = tables.Where(t => !bookings.Any(b => b.TableID == t.ID)).ToList();
            freeTables = freeTables.OrderByDescending(t => t.Capacity).ToList();

            return freeTables;
        }


        [HttpGet("free/sorted/asc")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<Table>>> GetFreeTablesSortedByCapacityAscending(string date, string timeFrom, string timeTo)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeFrom) || string.IsNullOrEmpty(timeTo))
            {
                return BadRequest("date, timeFrom, and timeTo are required.");
            }

            if (!DateTime.TryParse(date, out DateTime bookingDate))
            {
                return BadRequest("Invalid date format.");
            }

            TimeSpan from;
            TimeSpan to;

            if (!TimeSpan.TryParse(timeFrom, out from))
            {
                return BadRequest("Invalid timeFrom format.");
            }

            if (!TimeSpan.TryParse(timeTo, out to))
            {
                return BadRequest("Invalid timeTo format.");
            }


            var bookingDateWithoutTime = bookingDate.Date;

            var bookings = await _context.Bookings
                .Where(b => b.Date.Date == bookingDateWithoutTime && !(from >= b.TimeTo || to <= b.TimeFrom))
                .ToListAsync();


            var tables = await _context.Tables.ToListAsync();
            var freeTables = tables.Where(t => !bookings.Any(b => b.TableID == t.ID)).ToList();
            freeTables = freeTables.OrderByDescending(t => t.Capacity).ToList();
            freeTables = freeTables.OrderBy(t => t.Capacity).ToList();

            return freeTables;
        }



        // POST: api/Tables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Table>> PostTable(TableDTO newTable)
        {
            Table table = new Table
            {
                Name = newTable.Name,
                Capacity = newTable.Capacity,
            };
            if (_context.Tables == null)
          {
              return Problem("Entity set 'RestorauntDBContext.Tables'  is null.");
          }
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTable", new { id = table.ID }, table);
        }

        // DELETE: api/Tables/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            if (_context.Tables == null)
            {
                return NotFound();
            }
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
