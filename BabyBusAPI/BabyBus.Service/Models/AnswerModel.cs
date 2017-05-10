using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Model.Entities.FAQ;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Service.Models
{
    public class AnswerModel:Answer
    {
        public RoleType RoleType { get; set; }
        public string UserName { get; set; }
    }
}
