using System;

namespace BabyBus.Logic.Shared
{
	public class ECPostComment
	{		
			public string Title { get; set; }

			public string Abstract { get; set; }

			public int UserId { get; set; }

			public int CommentType { get; set; }

			public int CommentId { get; set; }

			public DateTime CommentCreateDate { get; set; }

			public long PostInfoId { get; set; }

			public int CommentUserId { get; set; }

			public string CommnetContent { get; set; }

			public string CommentUserName { get; set; }

	}
}

