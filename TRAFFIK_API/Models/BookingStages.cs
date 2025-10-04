namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a stage in the booking process.
    /// </summary>
    public class BookingStages
    {
        /// <summary>
        /// The unique identifier for the booking stage.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the related booking.
        /// </summary>
        public int BookingId { get; set; } // Foreign key to Bookings
        public Booking Booking { get; set; }

        /// <summary>
        /// The name of the stage.
        /// </summary>
        public string StageName { get; set; } // e.g "Exterior", "Interior", "Completed"
        public string Status { get; set; }


        /// <summary>
        /// The timestamp when the stage was recorded.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public int UpdatedByUserId { get; set; } // Staff who changed the stage
        public User UpdatedByUser { get; set; }
    }
}