namespace TRAFFIK_API.Models
{
    public class CarTypeServices
    {
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; } = null!;

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; } = null!;
    }
}
