using System;

namespace BabyBus.Logic.Shared
{
	public class ECCategory
	{
		public int Id { get; set; }

		public ECColumnType ColumnType { get; set; }

		public string Name { get; set; }

		public int Level { get; set; }

		public bool? Cancel { get; set; }
	}
}

