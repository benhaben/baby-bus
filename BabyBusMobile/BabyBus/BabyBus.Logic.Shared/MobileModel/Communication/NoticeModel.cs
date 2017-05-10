using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class NoticeModel
	{

		public override bool Equals(Object obj)
		{
			NoticeModel noticeModel = obj as NoticeModel; 
			if (noticeModel == null)
				return false;
			else {
				return this.NoticeId == noticeModel.NoticeId;
			}
		}

		public override int GetHashCode()
		{
			return this.NoticeId.GetHashCode(); 
		}

		public NoticeModel()
		{
			IsReaded = false;
		}

		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		[Unique]
		public long NoticeId { get; set; }

		public long KindergartenId { get; set; }

		public long ClassId { get; set; }

		public NoticeType NoticeType { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime CreateTime { get; set; }

		public string ThumPics { get; set; }

		public string NormalPics { get; set; }

		public int ReceiverNumber { get; set; }

		public int FavoriteCount { get; set; }

		public int ReadedCount { get; set; }

		public int ConfirmedCount { get; set; }

		public long UserId { get; set; }

		public string RealName { get; set; }

		public bool IsReaded { get; set; }

		public int ClassCount{ get; set; }

		public int KindergartenCount{ get; set; }

		public string HeadImage{ get; set; }

		public bool IsHtml{ get; set; }

		[Ignore]
		public List<int> ChildID{ get; set; }

		//show in the UI
		public string ChildrenName{ get; set; }

		[Ignore]
		public int ImageCount { get; set; }

		[Ignore]
		public string FirstImage {
			get { 
				string name;
				string pathAndName;
				if (!string.IsNullOrEmpty(NormalPics)) {
					name = NormalPics.Split(',')[0];
					if (!string.IsNullOrEmpty(name)) {
//                        pathAndName = Constants.ThumbServerPath + name + Constants.ThumbRule;
						pathAndName = name;

					} else {
						pathAndName = "";
					}
				} else {
					pathAndName = "";
				}
				return pathAndName;
			}
		}

		public string Abstract{ get; set; }

		[Ignore]
		public string AbstractDisplay {
			get {
				if (string.IsNullOrEmpty(Abstract)) {
					if (!string.IsNullOrEmpty(Content)) {
						if (Content.Length >= 18)
							return Content.Substring(0, 18) + "...";
						else
							return Content;
					} else {
						return string.Empty;
					}
				} else {
					if (Abstract.Length >= 18)
						return Abstract.Substring(0, 18) + "...";
					else
						return Abstract;
				}
			}
		}

		[Ignore]
		public string AbstractDisplayForiOS {
			get { 
				if (string.IsNullOrEmpty(Abstract)) {
					return Content;
				} else {
					return Abstract;
				}
			}
		}

		[Ignore]
		public List<string> ImageList {
			get {
				if (!string.IsNullOrEmpty(NormalPics)) {
					return new List<string>(NormalPics.Split(','));
				} else {
					return null;
				}
			}
		}
	}
}
