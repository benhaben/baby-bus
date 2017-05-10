using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.Article.Enum;

namespace BabyBus.Model.Entities.Article
{
    /// <summary>
    /// 消息类
    /// </summary>
    [Table("Notice")]
    public class Notice
    {
        public Notice() {
            ThumPics = string.Empty;
        }
        [Key]
        public long NoticeId { get; set; }
        [Index]
        [Required]
        public int KindergartenId { get; set; }
        [Index]
        [Required]
        public int ClassId { get; set; }
        [Index]
        [Required]
        public int UserId { get; set; }
        [Index]
        [Required]
        public NoticeType NoticeType { get; set; }
        [Required]
        [MaxLength(40)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        [Required]
        public DateTimeOffset CreateTime { get; set; }
        [MaxLength(1000)]
        public string ThumPics { get; set; }
        [MaxLength(1000)]
        public string NormalPics { get; set; }
        [Required]
        public int ReceiverNumber { get; set; }
        [Required]
        public int FavoriteCount { get; set; }
        [Required]
        public int ReadedCount { get; set; }
        [Required]
        public int ConfirmedCount { get; set; }
    }
}
