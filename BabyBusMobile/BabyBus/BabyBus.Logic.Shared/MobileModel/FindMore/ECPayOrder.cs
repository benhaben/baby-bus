using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	public class ECPayOrder
	{
		public string Title { get; set; }

		public string Abstract { get; set; }

		public string LessonTime { get; set; }

		public DateTime BeginDate { get; set; }

		public string Address { get; set; }

		public string ImageUrls { get; set; }

		public int InvolvedCount { get; set; }

		public int TotalCount { get; set; }

		public decimal Rating { get; set; }

		public decimal CurrentPrice { get; set; }

		public int PostInfoId { get; set; }

		public int ChildId { get; set; }

		public int Status { get; set; }

		public int UserId { get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public string ReviewContent { get; set; }

		public int ReviewRating { get; set; }

		public int ReviewId { get; set; }

		public DateTime? ReviewCreateDate { get; set; }

		public string OrderNumber { get; set; }

		public int ColumnType{ get; set; }

		public bool IsPaid{ get { return Status == 2; } }

		public bool IsReviewed{ get { return ReviewId != 0; } }

		[Ignore]
		public string Enroll {
			get { 
				return string.Format("已报{0}/{1}", InvolvedCount, TotalCount);
			}
		}

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
	}
}

