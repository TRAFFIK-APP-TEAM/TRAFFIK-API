namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// </summary>
    public class Notifications
    {
        /// <summary>
        /// The unique identifier for the notification.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who receives the notification.
        /// </summary>
        public int UserId { get; set; } // foreign key

        /// <summary>
        /// The ID of the related booking, if applicable.
        /// </summary>
        public int? BookingId { get; set; } //NULLBALE -can hold an int value or none

        //public string Title { get; set; }

        /// <summary>
        /// The message content of the notification.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The date and time the notification was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Indicates whether the notification has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// The type or category of the notification.
        /// </summary>
        public string Type { get; set; }

        public User User { get; set; }
        public Booking Booking { get; set; }
    }
}