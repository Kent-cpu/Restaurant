using Microsoft.EntityFrameworkCore;
using RestorauntAPI.Data.Models;

namespace RestorauntAPI.Data
{
    public class RestorauntDBContext: DbContext
    {
        public RestorauntDBContext(DbContextOptions<RestorauntDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Table> Tables { get; set; } 
        public DbSet<Booking> Bookings { get; set; }
    }
}
