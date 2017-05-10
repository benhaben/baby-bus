
using Cirrious.MvvmCross.Plugins.Messenger;


namespace BabyBus.Logic.Shared
{
    public class ChildMessage : MvxMessage
    {
        public ChildMessage(object sender, ChildModel child)
            : base(sender)
        {
            Child = child;
        }

        public ChildModel Child { get; private set; }
    }
}
