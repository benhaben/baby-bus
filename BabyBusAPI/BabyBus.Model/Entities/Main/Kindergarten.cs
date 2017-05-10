using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace BabyBus.Model.Entities.Main
{
    [Table("Kindergarten")]
    public class Kindergarten
    {
        [Key]
        public int KindergartenId { get; set; }
        [Required]
        [MaxLength(40)]
        public string KindergartenName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [MaxLength(20)]
        public string City { get; set; }
        [Required]
        public int KindergartenCount { get; set; }
        //[Required]
        public bool Cancel { get; set; }
    }
}
