namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a type of car (e.g., Sedan, SUV, Hatchback).
    /// </summary>
    public class CarType
    {
        /// <summary>
        /// The unique identifier for the car type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the car type.
        /// </summary>
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
