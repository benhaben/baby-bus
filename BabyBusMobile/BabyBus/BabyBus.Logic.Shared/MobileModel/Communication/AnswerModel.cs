using System;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class AnswerModel : Object
	{
		[PrimaryKey]
		public int AnswerId { get; set; }

		public long UserId { get; set; }

		public int QuestionId { get; set; }

		public string Content { get; set; }

		public virtual DateTime CreateTime { get; set; }

		public string CreateTimeString { get { return LogicUtils.DateTimeString(CreateTime); } }

		public virtual string RealName { get; set; }

		public virtual string ImageName { get; set; }

		public RoleType RoleType { get; set; }

		[Ignore]
		public string ContentAbstract {
			get {
				if (Content.Length > 18) {
					return Content.Substring(0, 18) + "..."; 
				} else {
					return Content;
				}
			}
		}

		[Ignore]
		public bool IsMyself {
			get {
				return RoleType == BabyBusContext.RoleType;
			}
		}
	}
}
