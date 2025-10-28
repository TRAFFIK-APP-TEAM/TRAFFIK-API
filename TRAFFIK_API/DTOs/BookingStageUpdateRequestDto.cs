using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.DTOs
{
    /// <summary>
    /// DTO for receiving booking stage update requests from the frontend
    /// </summary>
    public class BookingStageUpdateRequestDto
    {
        public int Id { get; set; }
        [Required]
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public string CurrentStage { get; set; } = string.Empty;
        public List<string> AvailableStages { get; set; } = new List<string>();
        [Required]
        public string SelectedStage { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}

