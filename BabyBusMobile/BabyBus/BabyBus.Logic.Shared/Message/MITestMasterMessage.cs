
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
    public class MITestMasterMessage:MvxMessage
    {
        public MITestMasterMessage(object sender, MITestMaster master)
            : base(sender)
        {
            Master = master;
        }

        public MITestMaster Master{ get; private set; }
    }
}

