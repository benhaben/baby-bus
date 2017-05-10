using System;

namespace BabyBus.Logic.Shared
{
	public class MI_TestMasterModel
	{
		public MI_TestMasterModel ()
		{
		}
		public string UserName { get; set; }

		public string ImageName { get; set; }

		public int ChildId { get; set; }

		public int ModalityId { get; set; }

		public int TotalTest { get; set; }

		public int CompletedTest { get; set; }

		public bool IsFinished { get; set; }
	}
}

