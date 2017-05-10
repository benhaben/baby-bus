using System;

namespace BabyBus.Logic.Shared
{
	public class MITestMaster
	{
		public MITestMaster()
		{
		}

		public bool IsMember {
			get;
			set;
		}

		public int TestMasterId { get; set; }

		public long ChildId { get; set; }

		public int TestRoleType { get; set; }

		public long ModalityId { get; set; }

		public int TotalTest { get; set; }

		public int CompletedTest { get; set; }

		public bool IsFinished { get; set; }

		public string ChildName { get; set; }

		public string ImageName { get; set; }

		public string ModalityName{ get; set; }

		public int  ModalityImageId{ get; set; }
	}
}

