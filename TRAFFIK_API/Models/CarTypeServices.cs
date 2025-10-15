namespace TRAFFIK_API.Models
{
    public class CarTypeServices
    {
        public string VehicleTypeId { get; set; }
        public Vehicle VehicleType { get; set; }

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; }
    }
}
