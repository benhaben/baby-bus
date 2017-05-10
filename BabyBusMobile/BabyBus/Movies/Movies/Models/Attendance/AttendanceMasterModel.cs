using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Models.Attendance {
    public class AttendanceMasterModel {
        [PrimaryKey]
        public int MasterId { get; set; }

        public DateTime CreateDate { get; set; }
        public int KindergartenId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int Total { get; set; }
        public int Attence { get; set; }
        public int UnAttence {
            get { return IsAttence?(Total - Attence):0; }
        }

        public bool IsAttence {
            get { return MasterId != 0; }
        }

        [Ignore]
        public List<int> AttChildren { get; set; }  
    }
}