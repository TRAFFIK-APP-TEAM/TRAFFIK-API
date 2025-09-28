namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a payment made for a booking.
    /// </summary>
    public class Payments
    {
        /// <summary>
        /// The unique identifier for the payment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the related booking.
        /// </summary>
        public int BookingId { get; set; } //foreign key to Booking Id

        /// <summary>
        /// The date and time the payment was made.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// The total amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The method used for payment (e.g., "Credit Card", "Cash").
        /// </summary>
        public string PaymentMethod { get; set; }

        //public string TransactionId { get; set; }   // External payment processor transaction ID

        /// <summary>
        /// Indicates whether the payment was successfully completed.
        /// </summary>
        public bool PaymentStatus { get; set; }
    }
}