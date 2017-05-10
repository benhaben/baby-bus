using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Main;
using Newtonsoft.Json;

namespace BabyBus.Model.Entities.Relation
{
    [Table("ParentChildRelation")]
    public class ParentChildRelation
    {
        [Key]
        public int ParentChildRelationId { get; set; }
        [Index]
        [Required]
        public int ChildId { get; set; }
        [Index]
        [Required]
        public int UserId { get; set; }
    }
}
