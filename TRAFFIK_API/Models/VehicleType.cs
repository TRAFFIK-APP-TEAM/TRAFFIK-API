using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFFIK_API.Models
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<CarTypeServices> CarTypeServices { get; set; } = new List<CarTypeServices>();
        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
        public ICollection<ServiceCatalog> Services { get; set; } = new List<ServiceCatalog>();
    }
}