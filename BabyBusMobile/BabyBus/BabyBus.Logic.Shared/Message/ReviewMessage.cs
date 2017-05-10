using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
	public class ReviewMessage : MvxMessage
	{
		public ReviewMessage(object sender, ECReview review)
			: base(sender)
		{
			ECReview = review;
		}

		public ECReview ECReview{ get; set; }
	}
}

