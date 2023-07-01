using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestorauntAPI.Data;
using RestorauntAPI.Data.DTO;
using RestorauntAPI.Data.Models;

namespace RestorauntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerRoot
    {
        private readonly RestorauntDBContext _context;

        public BookingsController(RestorauntDBContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.ToListAsync();
        }

        [HttpGet("mybookings")]
        [Authorize(Roles = "admin, User")]
        public async Task<ActionResult<IEnumerable<UserBookingsDTO>>> GetUserBookings()
        {
            var userID = GetCurrentUserID();
            // Проверяем, что userID не равен 0 (или другому значению, которое вы используете для обозначения недействительного или отсутствующего идентификатора пользователя)
            if (userID == 0)
            {
                return Forbid();
            }

            // Проверяем наличие пользователя с указанным ID
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ищем бронирования пользователя
            var bookings = await _context.Bookings
                .Include(b => b.Table)
                .Where(b => b.UserID == userID)
                .ToListAsync();

            // Создаем DTO объекты для передачи данных
            var userBookingsDTO = bookings.Select(b => new UserBookingsDTO
            {
                Date = b.Date,
                TimeFrom = b.TimeFrom,
                TimeTo = b.TimeTo,
                TableName = b.Table.Name
            }).ToList();

            return userBookingsDTO;
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutBooking(int id, BookingDTO newBooking)
        {
            Booking booking = new Booking
            {
                ID = id,
                UserID = newBooking.UserID,
                TableID = newBooking.TableID,
                Date = newBooking.Date,
                TimeFrom = TimeSpan.Parse(newBooking.TimeFrom),
                TimeTo = TimeSpan.Parse(newBooking.TimeTo),
            };
            if (id != booking.ID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO newBooking)
        {
            Booking booking = new Booking
            {
                UserID = newBooking.UserID,
                TableID = newBooking.TableID,
                Date = newBooking.Date,
                TimeFrom = TimeSpan.Parse(newBooking.TimeFrom),
                TimeTo = TimeSpan.Parse(newBooking.TimeTo),
            };
            if (_context.Bookings == null)
          {
              return Problem("Entity set 'RestorauntDBContext.Bookings'  is null.");
          }
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.ID }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
