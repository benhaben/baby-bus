using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Message
{
    public class ChildMessage : MvxMessage
    {
        public ChildMessage(object sender,ChildModel child) : base(sender) {
            Child = child;
        }

        public ChildModel Child { get; private set; }
    }
}
