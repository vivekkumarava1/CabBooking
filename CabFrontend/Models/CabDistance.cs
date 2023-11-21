using Newtonsoft.Json;

namespace CabFrontend.Models
{
    public class CabDistance
    {
        public Models.Cab Cab { get; set; }
        
        public double DistanceInKm { get; set; }
        public double Rating { get; set; }

        [JsonProperty("id")]
        public string cabId { get; set; }
    }
}
