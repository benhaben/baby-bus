

namespace BabyBus.Logic.Shared
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
            Teacher = new UserModel();
        }

        public ChildModel Child { get; set; }

        public KindergartenModel Kindergarten { get; set; }

        public KindergartenClassModel KindergartenClass { get; set; }

        public UserModel Teacher { get; set; }
    }
}
