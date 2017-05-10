using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
	public class CommentMessage : MvxMessage
	{
		public CommentMessage(object sender, ECComment comment)
			: base(sender)
		{
			ECComment = comment;
		}

		public ECComment ECComment{ get; set; }
	}
}

