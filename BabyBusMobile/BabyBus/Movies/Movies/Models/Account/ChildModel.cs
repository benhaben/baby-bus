using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using Cirrious.MvvmCross.Plugins.Sqlite;
using SQLiteNetExtensions.Attributes;

namespace BabyBus.Models
{
    /// <summary>
    /// 孩子信息
    /// </summary>
    public class ChildModel
    {
		public ChildModel(){
		}

		public ChildModel(string childName){
			ChildName = childName;
		}

        [PrimaryKey]
        public int ChildId { get; set; }

        public int ClassId { get; set; }

        /// <summary>
        /// 看起来每个小孩都需要密保卡？
        /// </summary>
        public string CardPassword { get; set; }

        public int KindergartenId { get; set; }

        public string ChildName { get; set; }

		public string Description { get; set; }

        /// <summary>
        /// 1-男孩；0-女孩
        /// </summary>
        public int Gender { get; set; }

        public DateTime Birthday { get; set; }

        [Ignore]
        public byte[] Image { get; set; }

        public string ImageName { get; set; }

		public bool IsSelect { get; set; }
        public string GenderName {
            get {
                return Gender == 1 ? "男" : "女";
            }
        }

        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
    }
}
