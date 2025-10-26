namespace TRAFFIK_API.DTOs
{
    public class ServiceHistoryDto
    {
        public string VehicleLicensePlate { get; set; } = string.Empty;
        public int ServiceCatalogId { get; set; }
        public int UserId { get; set; }

    }
}
