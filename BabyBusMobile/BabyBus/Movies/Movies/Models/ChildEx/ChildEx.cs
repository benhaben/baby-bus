using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Models.ChildEx
{
    public class ChildEx
    {
        public int Id { get; set; }

		public string SlideDisplay { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime InDate { get; set; }

        public ChildExType ChildExType { get; set; }

    }

    public enum ChildExType
    {
        幼教经验,
        健康经验,
        幼教服务
    }

}
