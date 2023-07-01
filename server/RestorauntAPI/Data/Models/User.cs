namespace RestorauntAPI.Data.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
