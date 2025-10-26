using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.DTOs
{
    public class VehicleUpdateDto
    {
        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public int VehicleTypeId { get; set; }

        public string Color { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }
    }
}
