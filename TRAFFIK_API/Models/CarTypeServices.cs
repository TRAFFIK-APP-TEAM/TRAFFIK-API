namespace TRAFFIK_API.Models
{
    public class CarTypeServices
    {
        public int CarTypeId { get; set; }
        public VehicleType CarType { get; set; } = null!;

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; } = null!;
    }
}
