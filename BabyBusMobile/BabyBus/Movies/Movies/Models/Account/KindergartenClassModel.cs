using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;
using SQLiteNetExtensions.Attributes;

namespace BabyBus.Models
{
    public class KindergartenClassModel
    {
		public KindergartenClassModel(){
		}
		public KindergartenClassModel(int id, string name, string desc = ""){
            ClassId = id;
			ClassName = name;
			Description = desc;
		}
        [PrimaryKey]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Name { get { return ClassName; } }
        public string Description { get; set; }

        public int KindergartenId { get; set; }
        public int ClassCount { get; set; }

        [Ignore]
        public List<ChildModel> ChildList { get; set; }

        [Ignore]
        public bool IsNull {
            get { return ClassId == 0; }
        }

        public override string ToString() {
            return Name;
        }
    }
}
