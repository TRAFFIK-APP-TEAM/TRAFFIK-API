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

        public string ImageUrl { get; set; }

        /// <summary>
        /// The manufacturing year of the vehicle.
        /// </summary>
        public int Year { get; set; }
        public string VehicleType { get; set; }
        //public VehicleTpye VehicleType { get; set; }
        public string Color { get; set; }

        public User User { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<CarModelService> CarModelServices { get; set; } = new List<CarModelService>();
        public ICollection<ServiceHistory> ServiceHistories { get; set; } = new List<ServiceHistory>();
        public ICollection<CarTypeServices> CarTypeServices { get; set; } = new List<CarTypeServices>();
        //public ICollection<ServiceCatalog>Services { get; set; } = new List<ServiceCatalog>();

    }
}