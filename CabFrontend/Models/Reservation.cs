namespace CabFrontend.Models
{
    public class Reservation
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string CabId { get; set; }
        public string Destination { get; set; }
        public bool isPaid {  get; set; }
        public bool isRated {  get; set; }
        public double tripDistance {  get; set; }
        public double tripAmount {  get; set; }
        public DateTime BookingTime { get; set; }
        public ReservationStatus Status { get; set; }
    }
    public enum ReservationStatus
    {

        Confirmed,
        Cancelled,
        Completed

    }
}

