namespace CabFrontend.Models
{
    public class CalculateDistanceAndNearestCabsModel
    {
        public double CalculatedDistance { get; set; }
        public List<CabDistance> NearestCabs { get; set; }
    }
}
