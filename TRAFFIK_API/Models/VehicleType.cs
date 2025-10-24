namespace TRAFFIK_API.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        
        /// <summary>
        /// Navigation property to car models of this type.
        /// </summary>
        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();

        /// <summary>
        /// Navigation property to services available for this car type.
        /// </summary>
        public ICollection<CarTypeServices> CarTypeServices { get; set; } = new List<CarTypeServices>();

        /// <summary>
        /// Navigation property to services offered for this car type.
        /// </summary>
        public ICollection<ServiceCatalog> Services { get; set; } = new List<ServiceCatalog>();
    }
}
