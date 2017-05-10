using System;

namespace BabyBus.Logic.Shared
{
	public class ShareModel
	{
		public long Id{ get; set; }

		/// <summary>
		/// 1-发现；2-信息通知
		/// </summary>
		/// <value>The type of the content.</value>
		public int ContentType { get; set; }

		public string Title{ get; set; }

		public string Description{ get; set; }
	}
}

