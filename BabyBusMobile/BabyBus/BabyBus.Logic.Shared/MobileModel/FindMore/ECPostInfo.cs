using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	public class ECPostInfo
	{
		[PrimaryKey]
		public int PostInfoId { get; set; }

		public string Abstract { get; set; }

		public string Title { get; set; }

		public long CategoryId { get; set; }

		public string CategoryName { get; set; }

		public int ColumnType { get; set; }

		public int CommentCount { get; set; }

		public DateTime CreateDate { get; set; }

		public decimal MarketPrice { get; set; }

		public decimal CurrentPrice { get; set; }

		public string Description { get; set; }

		public string ImageUrls { get; set; }

		public int InvolvedCount { get; set; }

		public string LessonTime { get; set; }

		public float Rating { get; set; }

		public int Status { get; set; }

		public int TotalCount { get; set; }

		public int UserId { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Html { get; set; }

		public int PraiseCount { get; set; }

		public string ImageName{ get; set; }

		public string RealName { get; set; }

		[Ignore]
		public string FirstImage {
			get { 
				string name;
				string pathAndName;
				if (!string.IsNullOrEmpty(ImageUrls)) {
					name = ImageUrls.Split(',')[0];
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

		[Ignore]
		public string DisplayAbstract {
			get {
				if (!string.IsNullOrEmpty(Abstract)) {
					if (Abstract.Length >= 40)
						return Abstract.Substring(0, 40) + "...";
					else
						return Abstract;
				} else {
					return string.Empty;
				}
			}
		}

		[Ignore]
		public string DisplayTitle {
			get {
				if (!string.IsNullOrEmpty(Title)) {
					if (Title.Length >= 11)
						return Title.Substring(0, 11) + "...";
					else
						return Title;
				} else {
					return string.Empty;
				}
			}
		}

		[Ignore]
		public string Enroll {
			get { 
				return string.Format("已报{0}/{1}", InvolvedCount, TotalCount);
			}
		}
	}
}

