using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models.Communication;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Message
{
    public class QuestionMessage : MvxMessage
    {
        public QuestionModel Question { get; private set; }

        public QuestionMessage(object sender,QuestionModel question) : base(sender) {
            Question = question;
        }


    }
}
