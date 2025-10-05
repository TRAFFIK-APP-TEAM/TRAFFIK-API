namespace TRAFFIK_API.Models
{
    public class CarTypeServices
    {
        public int CarTypeId { get; set; }
        public CarType CarType { get; set; }

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; }
    }
}
