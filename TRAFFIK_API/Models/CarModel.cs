using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a car model owned by a user.
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// The unique identifier for the car model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who owns the car.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the car type.
        /// </summary>
        public int CarTypeId { get; set; }

        /// <summary>
        /// The make of the car.
        /// </summary>
        public string Make { get; set; } = string.Empty;

        /// <summary>
        /// The model of the car.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// The plate number of the car.
        /// </summary>
        public string PlateNumber { get; set; } = string.Empty;

        /// <summary>
        /// The manufacturing year of the car.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Navigation property to the user who owns this car.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to the car type.
        /// </summary>
        public VehicleType CarType { get; set; } = null!;

        /// <summary>
        /// Navigation property to bookings for this car.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        /// <summary>
        /// Navigation property to services available for this car model.
        /// </summary>
        public ICollection<CarModelService> CarModelServices { get; set; } = new List<CarModelService>();

        /// <summary>
        /// Navigation property to service history for this car.
        /// </summary>
        public ICollection<ServiceHistory> ServiceHistories { get; set; } = new List<ServiceHistory>();
    }
}
