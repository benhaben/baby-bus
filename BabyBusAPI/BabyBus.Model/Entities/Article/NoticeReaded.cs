using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Article
{
    /// <summary>
    /// 消息已读
    /// </summary>
    [Table("NoticeReaded")]
    public class NoticeReaded
    {
        [Key]
        public int NoticeReadedId { get; set; }
        [Index]
        public int NoticeId { get; set; }
        [Index]
        public int UserId { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
