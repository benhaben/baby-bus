using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.Models
{
    public class ChildModel : Child
    {
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        public bool IsSelect { get; set; }
    }
}
