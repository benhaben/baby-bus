using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    public class AnswerTheQuestionController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AnswerTheQuestionController));
        //
        // GET: /AnswerTheQuestion/

        private ApiResponser AnswerTheQuestion(Question value)
        {
            var responer = new ApiResponser(true);
            try{
                using (var db = new BabyBus_Entities())
                {
                    var answerQus = from q in db.Question
                                    join c in db.Child on q.ChildId equals c.ChildId
                                    where q.QuestionId == value.QuestionId
                                    select new
                                    {
                                        userNames = c.ChildName,
                                        year = q.CreateTime.Year,
                                        month = q.CreateTime.Month,
                                        days = q.CreateTime.Day,
                                        title = q.Content,
                                        type = q.QuestionType,
                                        answers = from qs in db.Question
                                                  join a in db.Answer on qs.QuestionId equals a.QuestionId
                                                  join ch in db.Child on qs.ChildId equals ch.ChildId
                                                  join cl in db.Classes on ch.ClassId equals cl.ClassId
                                                  join us in db.Users on a.UserId equals us.UserId
                                                  where qs.QuestionId == value.QuestionId
                                                  select new
                                                  {
                                                      contents = a.Content,
                                                      years = a.CreateTime.Value.Year,
                                                      months = a.CreateTime.Value.Month,
                                                      dayes = a.CreateTime.Value.Day,
                                                      className = cl.ClassName,
                                                      userName = us.RealName,
                                                  }

                                    };
                    responer.Attach = new
                    {
                        AnswerQus = answerQus.ToList(),
                    };
                }
            }catch(Exception e){
                responer.Status = false;
                responer.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responer;
            }
            return responer;
        }
     

        [System.Web.Http.HttpPost]

        public ApiResponser Post(Question value)
        {
            return AnswerTheQuestion(value);
        }
	}
}