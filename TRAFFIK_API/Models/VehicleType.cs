using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFFIK_API.Models
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; }
    }
}