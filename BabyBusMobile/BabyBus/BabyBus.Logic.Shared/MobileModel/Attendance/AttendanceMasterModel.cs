using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
    public class AttendanceMasterModel
    {
        [PrimaryKey]
        public long MasterId { get; set; }

        public DateTime CreateDate { get; set; }

        public long KindergartenId { get; set; }

        public long ClassId { get; set; }

        public string ClassName { get; set; }

        public long Total { get; set; }

        public long Attence { get; set; }

        public long UnAttence
        {
            get { return IsAttence ? (Total - Attence) : 0; }
        }

        public bool IsAttence
        {
            get { return MasterId != 0; }
        }

        [Ignore]
        public List<long> AttChildren { get; set; }

    }
}