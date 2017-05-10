using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyBus.Model.Entities.Login
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(40)]
        public string AdminLoginName { get; set; }
        [Required]
        [MaxLength(40)]
        public string AdminLoginPassword { get; set; }
    }
}