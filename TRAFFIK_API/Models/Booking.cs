namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a booking made by a user for a specific service and vehicle.
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// The unique identifier for the booking.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who made the booking.
        /// </summary>
        public int UserId { get; set; } //Foreign key to User ID

        /// <summary>
        /// The ID of the service being booked.
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// The ID of the car model associated with the booking.
        /// </summary>
        public string VehicleLicensePlate { get; set; }

        /// <summary>
        /// The time of the booking.
        /// </summary>
        public TimeOnly BookingTime { get; set; }

        /// <summary>
        /// The date of the booking.
        /// </summary>
        public DateOnly BookingDate { get; set; }

        /// <summary>
        /// The current status of the booking.
        /// </summary>
        public string Status { get; set; } // e.g., "Pending", "Completed", "Cancelled"

 
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<BookingStages> BookingStages { get; set; } = new List<BookingStages>();

    }
}