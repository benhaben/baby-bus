using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Models.Communication {
    public enum QuestionType {
        AskforLeave = 0,
        NormalMessage = 1
    }

    public enum DisplayType {
        AskforLeave = 0,
        UnanswerQuestion = 1,
        AnsweredQuestion = 2
    }

    public class QuestionModel {
        [PrimaryKey]

        public int QuestionId { get; set; }

        public int ChildId { get; set; }

        public string Content { get; set; }

        public QuestionType QuestionType { get; set; }

        public DateTime CreateTime { get; set; }

        //      TODO：  家长的名字是不是更好
        public string ChildName { get; set; }

        public string AnswerContent {
            get {
                if (Answers == null)
                    return string.Empty;
                else {
                    return Answers.FirstOrDefault().Content;
                }
            }
        }

        public string TeacherName {
            get {
                if (Answers == null)
                    return string.Empty;
                else {
                    return Answers.FirstOrDefault().UserName;
                }
            }
        }

        [Ignore]
        public IList<AnswerModel> Answers { get; set; }

        public RoleType RoleType {
            get {
                return RoleType.Parent;
            }
        }

        public bool IsHaveAnswers {
            get {
                return !string.IsNullOrEmpty(AnswerContent);
            }
        }

        public string AnswerString {
            get {
                if (DisplayType == DisplayType.AskforLeave) {
                    return "请假";
                } else {
                    if (!string.IsNullOrEmpty(AnswerContent)) {
                        return "已回答";
                    } else {
                        return "未回答";
                    }
                }
            }
        }

        public DisplayType DisplayType {
            get {
                if (QuestionType == QuestionType.AskforLeave) {
                    return DisplayType.AskforLeave;
                }
                if (IsHaveAnswers) {
                    return DisplayType.AnsweredQuestion;
                } else {
                    return DisplayType.UnanswerQuestion;
                }
            }
        }
    }
}
