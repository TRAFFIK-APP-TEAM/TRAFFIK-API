namespace TRAFFIK_API.Models
{
    public class ServiceHistory
    {
        public int Id { get; set; }

        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; } = null!; 

        public int ServiceCatalogId { get; set; }
        public ServiceCatalog ServiceCatalog { get; set; }

        public int UserId { get; set; } 
        public User User { get; set; }

        public DateTime CompletedAt { get; set; }
    }

}
