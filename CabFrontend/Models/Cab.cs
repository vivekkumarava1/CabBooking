namespace CabFrontend.Models
{
    public class Cab
    {
        public string CabId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public double Longitude { get; set; }  // Longitude of the cab's current location
        public double Latitude { get; set; }   // Latitude of the cab's current location
        public bool IsAvailable { get; set; }
    }
}
