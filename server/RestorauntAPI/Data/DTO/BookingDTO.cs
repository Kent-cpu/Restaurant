namespace RestorauntAPI.Data.Models
{
    public class BookingDTO
    {
        public int UserID { get; set; }
        public int TableID { get; set; }
        public DateTime Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }
}
