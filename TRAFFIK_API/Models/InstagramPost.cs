using System.ComponentModel.DataAnnotations;

namespace TRAFFIK_API.Models
{
    public class InstagramPost
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string MediaUrl { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
