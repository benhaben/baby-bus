using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.Logic.Shared
{
	/// <summary>
	/// 用户信息
	/// </summary>
	public class UserModel
	{
		public UserModel()
		{
			UpdateStatus = true;
		}

		[PrimaryKey]
		public long UserId { get; set; }

		public string Cookie {
			get;
			set;
		}

		public int RoleId { get; set; }

		public string RoleName { get; set; }

		public string RealName { get; set; }

		public string LoginName { get; set; }

		public string Password { get; set; }

		public string Description { get; set; }

		public RoleType RoleType { get; set; }

		public string ImageName { get; set; }

		public int CityId { get; set; }

		public bool IsMember { get; set; }

		/// <summary>
		/// 对老师，园长有用
		/// </summary>
		public long KindergartenId { get; set; }

		public long ClassId { get; set; }

		public long ChildId { get; set; }

		public bool UpdateStatus{ get; set; }

		[Ignore]
		public KindergartenModel Kindergarten { get; set; }

		[Ignore]
		public KindergartenClassModel Class { get; set; }

		[Ignore]
		public ChildModel Child { get; set; }

		[Ignore]
		public bool IsCheckout { get; set; }

		[Ignore]
		public AuditType CheckoutAuditType { get; set; }

		[Ignore]
		public string CheckoutMemo { get; set; }

		[Ignore]
		public byte[] Image { get; set; }
	}
}
