namespace TRAFFIK_API.Models
{
    public class CarType
    {
        public int Id { get; set; }
        public string Type { get; set; } 

        public ICollection<ServiceCatalog> Services { get; set; } = new List<ServiceCatalog>();
        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
        public ICollection<CarTypeServices> CarTypeServices { get; set; } = new List<CarTypeServices>();
    }
}
