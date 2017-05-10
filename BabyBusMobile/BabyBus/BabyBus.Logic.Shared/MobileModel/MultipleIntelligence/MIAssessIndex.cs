using System;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
	public class MIAssessIndex
	{
		public MIAssessIndex ()
		{
		}

		public string Name{ get; set; }

		public double Weight{ get; set; }

		public int ModalityId{ get; set; }

		public List<MITestQuestion> MITestList{ get; set; }
	}
}

