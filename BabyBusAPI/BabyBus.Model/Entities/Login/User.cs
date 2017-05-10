using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Model.Entities.Login
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Index]
        public int WeChatInfoId { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(40)]
        public string LoginName { get; set; }
        [Index]
        [MaxLength(40)]
        public string RealName { get; set; }
        [Required]
        [MaxLength(40)]
        public string Password { get; set; }
        [Required]
        public RoleType RoleType { get; set; }
        [Index]
        [Required]
        public int KindergartenId { get; set; }
        [Index]
        [Required]
        public int ClassId { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }
        [Required]
        public DateTimeOffset CreateTime { get; set; }
        //[Required]
        public bool Cancel { get; set; }
    }
}
