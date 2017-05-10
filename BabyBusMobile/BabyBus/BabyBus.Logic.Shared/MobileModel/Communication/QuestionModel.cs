using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.Logic.Shared
{
   

	public enum DisplayType
	{
		AskforLeave = 0,
		UnanswerQuestion = 1,
		AnsweredQuestion = 2
	}

	public class QuestionModel
	{
        
		public override bool Equals(Object obj)
		{
			QuestionModel questionModel = obj as QuestionModel; 
			if (questionModel == null)
				return false;
			else {
				return this.QuestionId == questionModel.QuestionId;
			}
		}

		public override int GetHashCode()
		{
			return this.QuestionId.GetHashCode(); 
		}

		[PrimaryKey]
		public long QuestionId { get; set; }

		public long ChildId { get; set; }

		public string Content { get; set; }

		[Ignore]
		public string ContentWithDate { get { return Content + "\n" + BeginDateToEndDate; } }

		[Ignore]
		public string ContentAbstract {
			get { 
				if (Content.Length > 20) {
					return Content.Substring(0, 20) + "...";
				} else {
					return Content;
				}
			}
		}

		[Ignore]
		public String BeginDateToEndDate {
			get {
				if (QuestionType.AskforLeave == QuestionType) {
					var b = BeginDate.Date;
					var e = EndDate.Date;
					var day = e.Date.DayOfYear - b.Date.DayOfYear + 1;

					if (b == e) {
						return "请假时间：" + b.ToString("M月d日") + "共 1 天";
					} else {
						return "请假时间：从" + b.ToString("M月d日") + "上午 到 " + e.ToString("M月d日") + "下午共 " + day.ToString() + " 天";
					}
				} else {
					return "";
				}
			}
		}

		public QuestionType QuestionType { get; set; }

		public DateTime CreateTime { get; set; }

		public int SendUserId { get; set; }

		public string SenderImage { get; set; }

		public string ChildName { get; set; }

		public string SendUserName{ get; set; }

		public string ClassName{ get; set; }

		[Ignore]
		public List<long> Children{ get; set; }

		public string AnswerAbstract {
			get {
				if (Answers == null || Answers.Count == 0)
					return string.Empty;
				else {
					var answer = Answers.FirstOrDefault();
					return string.Format("{0}: {1}", answer.RealName, answer.ContentAbstract);
				}
			}
		}

		public string Userinfo {
			get { 
				switch (QuestionType) {
					case QuestionType.AskforLeave:
						return SendUserName + "(" + ChildName + "家长)";
					case QuestionType.NormalMessage:
						return SendUserName + "(" + ChildName + "家长)";
					case QuestionType.MasterMessage:
						return SendUserName + "(" + ClassName + ChildName + "家长)";
					case QuestionType.PersonalMessage:
						return SendUserName + "(" + ClassName + "班主任)";
					default:
						return string.Empty;
				}
			}
		}
		/* public string TeacherName
        {
            get
            {
                if (Answers == null || Answers.Count == 0)
                    return string.Empty;
                else
                {
                    var answer = Answers.FirstOrDefault();
                    return string.Format("{0}", answer.UserName);
                }
            }
        }*/

		//		private List<AnswerModel> _answers = new List<AnswerModel>();
		[Ignore]
		public List<AnswerModel> Answers{ get ; set; }

		public DateTime BeginDate{ get; set; }

		public DateTime EndDate{ get; set; }

		public bool HasAnswer { get; set; }

		public string AnswerString {
			get {
//                if (DisplayType == DisplayType.AskforLeave) {
//                    return "请假";
//                } else {
				if (HasAnswer) {
					return "已答复";
				} else {
					return "未答复";
				}
//                }
			}
		}

		public DisplayType DisplayType {
			get {
				if (HasAnswer) {
					return DisplayType.AnsweredQuestion;
				} else {
					return DisplayType.UnanswerQuestion;
				}
			}
		}

		public int GetQustionType()
		{
			if (QuestionType == QuestionType.AskforLeave) {
				return (int)QuestionType.AskforLeave;
			} else if (QuestionType == QuestionType.MasterMessage) {
				return (int)QuestionType.MasterMessage;
			} else if (QuestionType == QuestionType.NormalMessage) {
				return (int)QuestionType.NormalMessage;
			} else if (QuestionType == QuestionType.PersonalMessage) {
				return (int)QuestionType.PersonalMessage;
			} else {
				return -1;
			}
		}
	}
}
