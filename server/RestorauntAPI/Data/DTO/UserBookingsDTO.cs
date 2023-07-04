﻿namespace RestorauntAPI.Data.DTO
{
    public class UserBookingsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string TableName { get; set; }
    }
}
