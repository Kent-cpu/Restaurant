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
        [Authorize(Roles = "admin, User")]
        public async Task<ActionResult<IEnumerable<Table>>> GetFreeTables(string date, string timeFrom, string timeTo)
        {
            var userID = GetCurrentUserID();
            // Проверяем, что userID не равен 0 (или другому значению, которое вы используете для обозначения недействительного или отсутствующего идентификатора пользователя)
            if (userID == 0)
            {
                return Forbid();
            }
            // Проверяем наличие имени, даты, времени начала и времени окончания
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeFrom) || string.IsNullOrEmpty(timeTo))
            {
                return BadRequest("date, timeFrom, and timeTo are required.");
            }

            // Преобразуем дату из строки в объект DateTime
            if (!DateTime.TryParse(date, out DateTime bookingDate))
            {
                return BadRequest("Invalid date format.");
            }

            TimeSpan from;
            TimeSpan to;

            // Проверяем корректность формата времени начала
            if (!TimeSpan.TryParse(timeFrom, out from))
            {
                return BadRequest("Invalid timeFrom format.");
            }

            // Проверяем корректность формата времени окончания
            if (!TimeSpan.TryParse(timeTo, out to))
            {
                return BadRequest("Invalid timeTo format.");
            }

            // Отбрасываем время, оставляя только год, месяц и день
            var bookingDateWithoutTime = bookingDate.Date;

            // Ищем все бронирования на указанную дату и в заданном интервале времени
            var bookings = await _context.Bookings
                .Where(b => b.Date.Date == bookingDateWithoutTime && !(from >= b.TimeTo || to <= b.TimeFrom))
                .ToListAsync();

            // Получаем все столики с указанным именем
            var tables = await _context.Tables.ToListAsync();

            // Фильтруем столики, исключая забронированные на указанную дату и в заданном интервале времени
            var freeTables = tables.Where(t => !bookings.Any(b => b.TableID == t.ID)).ToList();

            return freeTables;
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Table>> GetTable(int id)
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

            return table;
        }

        // PUT: api/Tables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutTable(int id, TableDTO newTable)
        {
            Table table = new Table
            {
                ID = id,
                Name = newTable.Name,
                Capacity = newTable.Capacity,
            };
            if (id != table.ID)
            {
                return BadRequest();
            }

            _context.Entry(table).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
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

        private bool TableExists(int id)
        {
            return (_context.Tables?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
