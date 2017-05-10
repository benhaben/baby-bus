using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{

    public class RedirectMessage : MvxMessage
    {
        public RedirectMessage(object sender)
            : base(sender)
        {
        }

        //注意不同角色的tab页位置不同
        public string PageTag
        {
            get;
            set;
        }
    }
}

