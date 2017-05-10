using Cirrious.MvvmCross.Plugins.Messenger;
using System.Collections.Generic;



namespace BabyBus.Logic.Shared
{
    public class ChildrenMessage:MvxMessage
    {
        public ChildrenMessage(object sender, List<ChildModel> children)
            : base(sender)
        {
            Children = children;
        }

        public List<ChildModel> Children{ get; private set; }
    }
}

