using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.Attendance;

namespace BabyBus.Service.Models
{
    public class AttendanceModel : AttendanceMaster
    {
        public List<int> AttChildren { get; set; }
        public string ClassName { get; set; }
    }
}
