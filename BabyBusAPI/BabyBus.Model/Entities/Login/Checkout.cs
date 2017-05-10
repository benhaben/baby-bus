using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Model.Entities.Login
{
    /// <summary>
    /// 注册绑定表，由老师进行审批
    /// </summary>
    [Table("Checkout")]
    public class Checkout
    {
        public Checkout() {
            AuditType = AuditType.Pending;
        }

        [Key]
        public int CheckOutId { get; set; }
        [Required]
        public AuditType AuditType { get; set; }
        [Required]
        public RoleType RoleType { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string RealName { get; set; }
        [MaxLength(20)]
        [Column(TypeName = "CHAR")]
        public string VerifyCode { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [Required]
        public int KindergartenId { get; set; }
        [Required]
        public int ClassId { get; set; }
        public int ChildId { get; set; }
        [Required]
        [MaxLength(20)]
        public string ChildName { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateTimeOffset Birthday { get; set; }
        [Required]
        public DateTimeOffset CreateTime { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }
        [MaxLength(100)]
        public string Memo { get; set; }
    }
}
