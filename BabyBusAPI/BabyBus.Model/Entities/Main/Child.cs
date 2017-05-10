using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Runtime.CompilerServices;
using BabyBus.Model.Entities.Login;
using Newtonsoft.Json;

namespace BabyBus.Model.Entities.Main
{
    [Table("Child")]
    public class Child
    {
        [Key]
        public int ChildId { get; set; }
        [Index]
        [Required]
        public int KindergartenId { get; set; }
        [Index]
        [Required]
        public int ClassId { get; set; }
        [Index] 
        public int CardPasswordId { get; set; }
        [Index]
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
        //[Required]
        public bool Cancel { get; set; }
    }
}
