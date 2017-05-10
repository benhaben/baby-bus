using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Attendance
{
    [Table("AttendanceDetail")]
    public class AttendanceDetail
    {
        [Key]
        public int DetailId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        [Index]
        public int MasterId { get; set; }
        [Index]
        public int ChildId { get; set; }
    }
}
