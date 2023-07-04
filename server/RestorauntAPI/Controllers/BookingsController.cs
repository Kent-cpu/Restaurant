using Microsoft.AspNetCore.Authorization;
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
            var bookings = await _context.Bookings
                .Include(b => b.Table)
                .Include(b => b.User)
                .Select(b => new
                {
                    id = b.ID,
                    TableName = b.Table.Name,
                    Username = b.User.Username,
                    b.Date,
                }).ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("mybookings/{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<UserBookingsDTO>>> GetUserBookings(int userId)
        {
            // Проверяем наличие пользователя с указанным ID
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            DateTime today = DateTime.Today;
            // Ищем бронирования пользователя
            var bookings = await _context.Bookings
                .Include(b => b.Table)
                .Where(b => b.UserID == userId && b.Date >= today)
                .ToListAsync();

            // Создаем DTO объекты для передачи данных
            var userBookingsDTO = bookings.Select(b => new UserBookingsDTO
            {
                Id = b.ID,
                Date = b.Date,
                TimeFrom = b.TimeFrom,
                TimeTo = b.TimeTo,
                TableName = b.Table.Name
            }).ToList();

            return userBookingsDTO;
        }

        [HttpGet("mybookings/history/{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<UserBookingsDTO>>> GetUserBookingsHistory(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            var bookings = await _context.Bookings
                .Include(b => b.Table)
                .Where(b => b.UserID == userId)
                .ToListAsync();

            // Создаем DTO объекты для передачи данных
            var userBookingsDTO = bookings.Select(b => new UserBookingsDTO
            {
                Id = b.ID,
                Date = b.Date,
                TimeFrom = b.TimeFrom,
                TimeTo = b.TimeTo,
                TableName = b.Table.Name
            }).ToList();

            return userBookingsDTO;
        }


        // POST: api/Bookings
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<Booking>> PostBooking([FromBody] BookingDTO newBooking)
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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}