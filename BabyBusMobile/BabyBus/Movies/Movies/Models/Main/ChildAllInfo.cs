using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;

namespace BabyBus.Models.Main
{
    /// <summary>
    /// 孩子的综合信息，包括孩子基本信息，幼儿园信息，班级信息，班主任信息
    /// </summary>
    public class ChildAllInfo
    {
        public ChildAllInfo()
        {
            Child = new ChildModel();
            Kindergarten = new KindergartenModel();
            KindergartenClass = new KindergartenClassModel();
            Teacher = new User();
        }
        public ChildModel Child { get; set; }
        public KindergartenModel Kindergarten { get; set; }
        public KindergartenClassModel KindergartenClass { get; set; }
        public User Teacher { get; set; }
    }
}
