using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Models
{
    public class VersionModel
    {
        public int AppType { get; set; }
        public string AppName { get; set; }
        public string ApkName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string VerName { get; set; }
        public int VerCode { get; set; }
    }
}
