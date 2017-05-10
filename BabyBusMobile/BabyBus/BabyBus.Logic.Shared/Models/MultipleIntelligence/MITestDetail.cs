using System;

namespace BabyBus.Logic.Shared
{
	public class MITestDetail
	{
		public MITestDetail()
		{
		}

		public int TestDetailId { get; set; }

		public int ChildId { get; set; }

		public int TestRoleType { get; set; }

		public int UserId { get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public int TestQuestionId { get; set; }

		public int Score { get; set; }
	}
}

