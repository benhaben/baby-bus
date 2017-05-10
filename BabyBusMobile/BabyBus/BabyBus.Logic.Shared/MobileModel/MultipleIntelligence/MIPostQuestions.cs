using System;
using BabyBus.Logic.Shared;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
    public class MIPostQuestions
    {
        public MIPostQuestions()
        {
        }

        public long UserId{ get; set; }

        public long TestRoleType { get; set; }

        public long ChildId { get; set; }

        public long ModalityId { get; set; }

        public long TestMasterId{ get; set; }

        public List<MITestQuestion> MITestList { get; set; }
    }
}

