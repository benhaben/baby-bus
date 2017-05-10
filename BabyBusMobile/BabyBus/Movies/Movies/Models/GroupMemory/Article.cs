using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Helpers;

namespace BabyBus.Models.GroupMemory
{
    public class Article
    {
		public int ArticleID { get; set; }
        public int UserClassRelationID { get; set; }
        public int CreatTime { get; set; }
        public string ArticleType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        private DateTime _inDate;
        public DateTime InDate
        {
            get
            {
                if (_inDate != DateTime.MinValue)
                    return _inDate;
                else
                {
                    _inDate = Utils.ConvertIntDateTime(CreatTime);

                    return _inDate;
                }
            }
            set { _inDate = value; }
        }

        public string Images{ get; set; }


		public string Image1{ get; set; }
		public string Image2{ get; set; }
		public string Image3{ get; set; }

    }
}
