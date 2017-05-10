using System;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class User
	{

		public long UserId { get; set; }

		public string UserName { get; set; }

		public virtual string LoginName { get; set; }

		public string RealName { get; set; }

		public string PhoneNumber { get; set; }

		public RoleType RoleType{ get; set; }

		public int KindergartenId { get; set; }

		public int ClassId { get; set; }

		public int ChildId { get; set; }

		public string ImageName { get; set; }

		public bool Cancel { get; set; }

		public string Password{ get; set; }
	}
}

