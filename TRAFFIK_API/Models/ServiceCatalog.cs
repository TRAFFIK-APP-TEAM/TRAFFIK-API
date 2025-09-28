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
        public string Name { get; set; } // e.g "Oil Change"

        /// <summary>
        /// A detailed description of the service.
        /// </summary>
        public string Description { get; set; } // e.g "Complete oil change with premium oil"

        /// <summary>
        /// The price of the service.
        /// </summary>
        public decimal Price { get; set; } // e.g 79.99
    }
}