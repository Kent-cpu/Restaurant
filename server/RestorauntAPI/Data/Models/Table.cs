namespace RestorauntAPI.Data.Models
{
    public class Table
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
