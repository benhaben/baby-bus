using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyBus.Model.Entities.Relation
{
    [Table("NoticeChildRelation")]
    public class NoticeChildRelation
    {
        [Key]
        public int NoticeChildRelationId { get; set; }
        [Index]
        public int ClassNoticeId { get; set; }
        [Index]
        public int ChildId { get; set; }
    }
}
