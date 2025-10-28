namespace TRAFFIK_API.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ServiceCatalogId { get; set; }
        public string? VehicleLicensePlate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public DateOnly BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Navigation properties for display
        public string ServiceName { get; set; } = string.Empty;
        public string VehicleDisplayName { get; set; } = string.Empty;
    }
}