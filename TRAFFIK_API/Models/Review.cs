namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a review submitted by a user for a booking.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// The unique identifier for the review.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who submitted the review.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the related booking.
        /// </summary>
        public int BookingId { get; set; } // Foreign key to Booking

        /// <summary>
        /// The rating given by the user (e.g., 1 to 5).
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// The written comment provided by the user.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The date and time the review was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}