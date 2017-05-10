using System;

namespace BabyBus.Models.MultipleIntelligence
{
	public class MITestMaster
	{
		public MITestMaster()
		{
		}

		public int TestMasterId { get; set; }

		public int ChildId { get; set; }

		public int TestRoleType { get; set; }

		public int ModalityId { get; set; }

		public int TotalTest { get; set; }

		public int CompletedTest { get; set; }

		public bool IsFinished { get; set; }

		public string Name { get; set; }

		public string ImageName { get; set; }

		public bool IsMember{ get; set; }



		public string ModalityName{ get; set; }

		public int  ModalityImageId{ get; set; }
	}
}

