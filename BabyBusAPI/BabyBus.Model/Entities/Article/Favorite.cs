using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Article
{
    [Table("Favorite")]
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }
        [Index]
        public int NoticeId { get; set; }
        [Index]
        public int UserId { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
