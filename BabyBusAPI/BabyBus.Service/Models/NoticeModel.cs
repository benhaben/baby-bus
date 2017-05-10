using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.Article;
using BabyBus.Model.Entities.Article.Enum;

namespace BabyBus.Service.Models
{
    /// <summary>
    /// Notice Model, work with WebAPI
    /// </summary>
    public class NoticeModel : Notice
    {
        public List<string> Base64ImageList { get; set; }
        public string RealName { get; set; }
        public int ImageCount { get; set; }
        public NoticeViewType NoticeViewType { get; set; }
    }
}
