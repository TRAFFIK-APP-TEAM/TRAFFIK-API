namespace TRAFFIK_API.Models
{
    public class CarModelService
    {
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; } = null!;

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; } = null!;
    }
}