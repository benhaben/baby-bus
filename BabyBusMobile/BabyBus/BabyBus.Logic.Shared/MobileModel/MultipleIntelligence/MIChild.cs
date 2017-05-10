using System;

namespace BabyBus.Logic.Shared
{
	public class MIChild
	{
		public MIChild()
		{
			
		}

		public MIChild(string childName)
		{
			ChildName = childName;
		}

		public string ChildName{ get; set; }

		public string ImageName{ get; set; }
	}
}

