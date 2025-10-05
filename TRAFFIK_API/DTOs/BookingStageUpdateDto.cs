using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.DTOs
{
    public class BookingStageUpdateDto
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        public string StageName { get; set; } // e.g. "Exterior"
        [Required]
        public string Status { get; set; } // e.g. "Started", "Completed"
        [Required]
        public int UpdatedByUserId { get; set; } // Staff user ID

    }
}
