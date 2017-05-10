using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Models.Main
{
    public class ChildInfo
    {
        public int ChildID { get; set; }

        public int ClassID { get; set; }

        public int CardPasswordID { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string Birthday { get; set; }

        public string PhotoUrl { get; set; }

        public int CreatTime { get; set; }

        public int Age { get; set; }
        public double Score { get; set; }
        public double Weight { get; set; }
        public int Height { get; set; }
    }
}
