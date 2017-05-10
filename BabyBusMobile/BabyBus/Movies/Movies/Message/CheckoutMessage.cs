using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models.Account;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Message
{
    public class CheckoutMessage : MvxMessage
    {
        public CheckoutMessage(object sender,CheckoutModel checkout) : base(sender) {
            Checkout = checkout;
        }

        public CheckoutModel Checkout { get; private set; }
    }
}
