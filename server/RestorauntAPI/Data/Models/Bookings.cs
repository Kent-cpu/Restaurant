namespace RestorauntAPI.Data.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TableID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }


        public User User { get; set; }
        public Table Table { get; set; }
    }
}
