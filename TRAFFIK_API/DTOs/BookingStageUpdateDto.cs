namespace TRAFFIK_API.DTOs
{
    public class BookingStageUpdateDto
    {
        public int BookingId { get; set; }
        public string StageName { get; set; } // e.g. "Exterior"
        public string Status { get; set; } // e.g. "Started", "Completed"
        public int UpdatedByUserId { get; set; } // Staff user ID

    }
}
