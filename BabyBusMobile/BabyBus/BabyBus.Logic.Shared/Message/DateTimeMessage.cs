using System;
using Cirrious.MvvmCross.Plugins.Messenger;


namespace BabyBus.Logic.Shared
{
    public class DateTimeMessage : MvxMessage
    {
        public DateTimeMessage(object sender, DateTime date)
            : base(sender)
        {
            Date = date;
        }

        public DateTime Date{ get; private set; }
    }
}

