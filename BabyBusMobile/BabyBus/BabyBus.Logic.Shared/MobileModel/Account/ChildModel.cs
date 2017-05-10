using System;

using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	/// <summary>
	/// 孩子信息
	/// </summary>
	public class ChildModel
	{
		public ChildModel()
		{
		}

		public ChildModel(string childName)
		{
			ChildName = childName;
		}


		[PrimaryKey]
		public long ChildId { get; set; }

		public long ClassId { get; set; }

		public long KindergartenId { get; set; }

		public string ChildName { get; set; }

		public int AttendancedetialId { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// 1-男孩；2-女孩
		/// </summary>
		public int Gender { get; set; }

		public DateTime Birthday { get; set; }

		[Ignore]
		public byte[] Image { get; set; }

		public string ImageName { get; set; }

		public bool IsSelect { get; set; }

		public string GenderName {
			get {
				return Gender == 1 ? "男" : "女";
			}
		}

		public string ParentName { get; set; }

		public string PhoneNumber { get; set; }

		public bool IsAskForLeave{ get; set; }

		public bool IsRead{ get; set; }

		public bool IsMember{ get; set; }

		public bool HasHeadImage{ get; set; }

	}
}
