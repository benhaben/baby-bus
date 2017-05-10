using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyBus.Model.Entities.Main
{
    [Table("Class")]
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        [Required]
        [Index]
        public int KindergartenId { get; set; }
        [Required]
        [MaxLength(20)]
        public string ClassName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int ClassCount { get; set; }
        //[Required]
        public bool Cancel { get; set; }
    }
}