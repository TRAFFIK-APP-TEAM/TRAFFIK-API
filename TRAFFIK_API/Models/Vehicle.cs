using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.Models
{
    /*public enum VehicleTpe{
        Bike,
        Sedan,
        SUV,
        Minivane,
        Truck,
        Caravan,
        Boat    
    }*/
    /// <summary>
    /// Represents a vehicle owned by a user.
    /// </summary>
    public class Vehicle
    {
        [Key]
        public string LicensePlate { get; set; } //PK

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

        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// The manufacturing year of the vehicle.
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// The ID of the vehicle type.
        /// </summary>
        public int VehicleTypeId { get; set; }
        
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the user who owns this vehicle.
        /// </summary>
        public User User { get; set; } = null!;
        
        /// <summary>
        /// Navigation property to the vehicle type.
        /// </summary>
        public VehicleType VehicleType { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<ServiceHistory> ServiceHistories { get; set; } = new List<ServiceHistory>();

    }
}