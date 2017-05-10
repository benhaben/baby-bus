using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBus.Helpers;

namespace BabyBus.Models.Communication {
    public class AnswerModel {
        [PrimaryKey]
        public int AnswerId { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateTimeString { get { return Utils.DateTimeString(CreateTime); } }

        public string UserName { get; set; }

        public RoleType RoleType { get; set; }
    }
}
