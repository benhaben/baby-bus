using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.FAQ;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Service.Models
{
    public class QuestionModel:Question
    {
        public int UserId { get; set; }   
        public RoleType RoleType { get; set; }   
        public string AnswerContent { get; set; }
        public string ChildName { get; set; }
        public string TeacherName { get; set; }
        public IEnumerable<Answer> Answers { get; set; } 
    }
}
