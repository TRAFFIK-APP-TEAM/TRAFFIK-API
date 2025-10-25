namespace TRAFFIK_API.DTOs
{
    public class CarModelDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
