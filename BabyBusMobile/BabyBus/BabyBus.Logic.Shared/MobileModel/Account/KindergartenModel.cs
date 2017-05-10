using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
    public class KindergartenModel
    {
        public KindergartenModel()
        {
        }

        public KindergartenModel(int id, string name, string desc)
        {
            KindergartenId = id;
            KindergartenName = name;
            Description = desc;
        }

        [PrimaryKey]
        public long KindergartenId { get; set; }

        public string KindergartenName { get; set; }

        public string Name { get { return KindergartenName; } }

        public string Description { get; set; }

        public string City{ get; set; }

        [Ignore]
        public bool IsNull
        {
            get { return KindergartenId == 0; }
        }

        [Ignore]
        public List<KindergartenClassModel> KindergartenClassList { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
