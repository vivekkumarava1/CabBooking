namespace CabFrontend.Models
{
    public class BookingViewModel
    {
        public string CabId { get; set; }
        public double tripDistance {  get; set; }
        public double tripAmount { get; set; }
        public bool isPaid { get; set; }
        public string cabModel {  get; set; }
        public string cabRegistration { get; set; }
        public double UserCabDistance { get; set; }
    }
}
