using System;

namespace BabyBus
{
	public class CommentModel
	{
		[Key]
		public long CommentId { get; set; }
		[Index]
		public int UserId { get; set; }
		[Index]
		public int NoticeId { get; set; }
		[MaxLength(200)]
		public string Content { get; set; }
		public DateTimeOffset CreateTime { get; set; }
		public  int CommentType{get;set;}
		public int ChildId { get; set; }
		public int ParentId { get; set; }
	}
}

