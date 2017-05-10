using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	public class ECReview
	{
		public string Content { get; set; }

		public DateTime CreateDate { get; set; }

		public int PostInfoId { get; set; }

		public int Rating { get; set; }

		[PrimaryKey]
		public int ReviewId { get; set; }

		public long UserId { get; set; }

		public string RealName { get; set; }

		public string ImageName{ get; set; }
	}
}

