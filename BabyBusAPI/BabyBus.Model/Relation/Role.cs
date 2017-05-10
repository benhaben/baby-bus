using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyBus.Model.Entities.Relation
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [MaxLength(20)]
        public string RoleName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
