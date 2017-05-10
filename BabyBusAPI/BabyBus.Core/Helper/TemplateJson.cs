using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Core.Helper
{
    public class TemplateJson
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string topcolor { get; set; }
        public TemplateData data { get; set; }
    }

    public class TemplateData
    {
        public First first { get; set; }
        public Keyword1 keyword1 { get; set; }
        public Keyword2 keyword2 { get; set; }
        public Keyword3 keyword3 { get; set; }
        public Remark remark { get; set; }
    }

    public class First
    {
        public string value { get; set; }
        public string color { get; set; }
    }
    public class Keyword1
    {
        public string value { get; set; }
        public string color { get; set; }
    }
    public class Keyword2
    {
        public string value { get; set; }
        public string color { get; set; }
    }
    public class Keyword3
    {
        public string value { get; set; }
        public string color { get; set; }
    }
    public class Remark
    {
        public string value { get; set; }
        public string color { get; set; }
    }
}
