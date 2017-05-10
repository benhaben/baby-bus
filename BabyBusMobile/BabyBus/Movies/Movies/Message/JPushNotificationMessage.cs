using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Message {
   
    public class JPushNotificationMessage : MvxMessage {
        public JPushNotificationMessage(object sender)
            : base(sender) {
        }
    }
}

