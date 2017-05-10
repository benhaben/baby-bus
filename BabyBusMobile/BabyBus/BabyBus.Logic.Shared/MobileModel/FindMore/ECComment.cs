using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	public class ECComment
	{
		[PrimaryKey]
		public int CommentId { get; set; }

		public int PostInfoId { get; set; }

		public string Content { get; set; }

		public DateTime CreateDate { get; set; }

		public long UserId { get; set; }

		public string RealName{ get; set; }

		public int CommentType { get; set; }
	}
}

