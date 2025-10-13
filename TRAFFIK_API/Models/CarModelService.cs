namespace TRAFFIK_API.Models
{
    public class CarModelService
    {

        public int VehicleModelId { get; set; }
        public Vehicle VehicleModel { get; set; }

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; }

    }
}