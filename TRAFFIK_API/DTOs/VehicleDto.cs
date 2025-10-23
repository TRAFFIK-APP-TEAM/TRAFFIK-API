namespace TRAFFIK_APP.DTOs
{
    public class VehicleDto
    {
        public string LicensePlate { get; set; }
        public int UserId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ImageUrl { get; set; } // Base64 string
        public int Year { get; set; }
        public string VehicleType { get; set; }
        public string Color { get; set; }
    }
}