using System;

namespace BabyBus.Logic.Shared
{
	public class AdvertisementModel
	{
		public int Id { get; set; }

		public string NormalPics { get; set; }

		public int IsUsed { get; set; }

		public int UserId { get; set; }

		public DateTimeOffset CreateTime { get; set; }

		public string Description { get; set; }

		public string UserType { get; set; }

		public Guid Guid { get; set; }
	}
}

