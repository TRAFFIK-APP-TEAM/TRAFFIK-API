namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a vehicle owned by a user.
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// The unique identifier for the car model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who owns the vehicle.
        /// </summary>
        public int UserId { get; set; } // Foreign key to User (customer)

        /// <summary>
        /// The make of the vehicle.
        /// </summary>
        public string Make { get; set; } // e.g "Nissan"

        /// <summary>
        /// The model of the vehicle.
        /// </summary>
        public string Model { get; set; } // e.g "GTR35"

        /// <summary>
        /// The manufacturing year of the vehicle.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The license plate number of the vehicle.
        /// </summary>
        public string PlateNumber { get; set; }
    }
}