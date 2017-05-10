using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Model.Entities.Login
{
    [Table("CardPassword")]
    public class CardPassword
    {
        [Key]
        public int CardPasswordId { get; set; }
        [Required]
        [MaxLength(20)]
        [Column(TypeName = "CHAR")]
        public string CodeNumber { get; set; }
        [Required]
        [MaxLength(20)]
        [Column(TypeName = "CHAR")]
        public string VerifyCode { get; set; }
        [Required]
        public CardFlag Flag { get; set; }
        [Required]
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset ActiveTime { get; set; }
    }
}
