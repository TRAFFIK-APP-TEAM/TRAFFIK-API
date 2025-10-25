namespace TRAFFIK_API.Models
{
    /// <summary>
    /// Represents a booking made by a user for a specific service and vehicle.
    /// </summary>
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ServiceId { get; set; }
        public int CarModelId { get; set; }
        public int? ServiceCatalogId { get; set; }

        public TimeOnly BookingTime { get; set; }
        public DateOnly BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? VehicleLicensePlate { get; set; }

        public User? User { get; set; }
        public CarModel? CarModel { get; set; }
        public ServiceCatalog? ServiceCatalog { get; set; }

        public ICollection<Payments> Payments { get; set; } = new List<Payments>();
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<BookingStages> BookingStages { get; set; } = new List<BookingStages>();
    }

}