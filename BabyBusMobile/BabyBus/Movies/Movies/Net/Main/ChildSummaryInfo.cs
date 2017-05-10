using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Models.Main
{
    public class ChildSummaryInfo
    {
        public long ClassNoticeId { get; set; }
        public string ClassNoticeTitle { get; set; }
        public string ClassNoticeContent { get; set; }
        public DateTimeOffset ClassNoticeCreateTime { get; set; }
        public long KindergartenNoticeId { get; set; }
        public string KindergartenNoticeTitle { get; set; }
        public string KindergartenNoticeConent { get; set; }
        public DateTimeOffset KindergartenNoticeCreateTime { get; set; }
        public string Pics { get; set; }
    }
}
