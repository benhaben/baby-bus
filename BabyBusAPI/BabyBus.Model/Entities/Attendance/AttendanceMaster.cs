using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Attendance
{
    [Table("AttendanceMaster")]
    public class AttendanceMaster
    {
        [Key]
        public int MasterId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Index]
        public int KindergartenId { get; set; }
        [Index]
        public int ClassId { get; set; }
        public int Total { get; set; }
        public int Attence { get; set; }
    }
}
