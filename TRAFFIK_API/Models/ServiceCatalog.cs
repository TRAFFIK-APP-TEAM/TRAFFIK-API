namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a service offered in the catalog.
    /// </summary>
    public class ServiceCatalog
    {
        /// <summary>
        /// The unique identifier for the service.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the service.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A detailed description of the service.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the service.
        /// </summary>
        public decimal Price { get; set; } // e.g 67

        /// <summary>
        /// The vehicle type this service is associated with.
        /// </summary>
        public int? VehicleTypeId { get; set; }

        public VehicleType? VehicleType { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}