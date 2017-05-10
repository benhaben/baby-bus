using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
   
	public class JPushNotificationMessage : MvxMessage
	{
		public JPushNotificationMessage(object sender)
			: base(sender)
		{
		}

		public long Id {
			get;
			set;
		}

		public string Tag {
			get;
			set;
		}

		public bool IsHtml {
			get;
			set;
		}
	}
}

