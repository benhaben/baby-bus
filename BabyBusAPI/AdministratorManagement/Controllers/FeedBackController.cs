using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    public class FeedBackController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(FeedBackController));

        public enum FeedBack
        {
            sendFeedback = 1,
            FeedbackDetail = 2,
            handlling = 3,
            handled = 4,
            selectFeedback = 5,
        }
        public class feedBack{
            public FeedBack recordType { get; set; }
            public int UserId { get; set; }
            public string Content { get; set; }
            public string CheckBoxHandlling { get; set; }
            public string CheckedBoxHandled { get; set; }
            public int StatusTypes { get; set; }
        }
        //
        // GET: /FeedBack/

        private ApiResponser FeedBackDetail(feedBack value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var feedBdInfo = from fb in db.Feedbacks
                                     group fb by fb.Status into gb
                                     select new
                                     {
                                         key = gb.Key,
                                         Status = gb,
                                         feedbackDetailInfo = from f in db.Feedbacks
                                                              join u in db.Users on f.UserId equals u.UserId
                                                              where f.Status == gb.Key
                                                              orderby f.CreatTime descending
                                                              select new
                                                              {
                                                                  FeedbackId = f.FeedbackId,
                                                                  f.Content,
                                                                  year = f.CreatTime.Year,
                                                                  month = f.CreatTime.Month,
                                                                  days = f.CreatTime.Day,
                                                                  userName = u.RealName,
                                                                  Phone = u.Phone,
                                                                  status = f.Status,
                                                              },
                                     };
                    responser.Attach = new
                    {
                        FeedBackInfo = feedBdInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal("login failure", e);
                return responser;
            }
            return responser;
        }
        private ApiResponser FeedbackInfo(feedBack value)
        {
            var responser = new ApiResponser(true);
            var time = System.DateTime.Now;
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var newFeedBack = new Feedback
                    {
                        UserId = value.UserId,
                        Content = value.Content,
                        CreatTime = time,
                        Type = "Web",
                        Name = " 贝贝巴士后台网站",
                        Status = 0,
                    };
                    db.Feedbacks.Add(newFeedBack);
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        Status = 0,
                    };
                }
            }
            catch(Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal("login failure", e);
                return responser;
            }
            return responser;
        }

        private ApiResponser Handlling(feedBack value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var feedBackInfo = from fb in db.Feedbacks select fb;
                    Dictionary<int, Feedback> mapCheckBoxHandlling = new Dictionary<int, Feedback>();
                    if (feedBackInfo != null && feedBackInfo.Count() > 0)
                    {
                        foreach(var info in feedBackInfo){
                            if (info == null)
                            {
                                continue;
                            }
                            if (!mapCheckBoxHandlling.ContainsKey(info.FeedbackId))
                            {
                                mapCheckBoxHandlling.Add(info.FeedbackId, info);
                            }
                        }
                    }
                    if (value.CheckBoxHandlling != null && value.CheckBoxHandlling != "")
                    {
                        var feedbackHandlling = value.CheckBoxHandlling;
                        string[] feedbackHandllingStatus = feedbackHandlling.Split(new Char[] { ',' });
                        for (var i = 0; i < feedbackHandllingStatus.Length; i++)
                        {
                            if (feedbackHandllingStatus[i] == "")
                            {
                                break;
                            }
                            var feedId = int.Parse(feedbackHandllingStatus[i]);
                            if (mapCheckBoxHandlling.ContainsKey(feedId))
                            {
                                Feedback feedIdStatus = mapCheckBoxHandlling[feedId];
                                if ( feedIdStatus.Status == 1)
                                {
                                    continue;
                                }
                                else if (feedIdStatus.Status == 2 || feedIdStatus.Status == 0)
                                {
                                    feedIdStatus.Status = 1;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                    responser.Attach = new
                    {
                        HandleType = 0,
                    };

                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal("login failure", e);
                return responser;
            }
            return responser;
        }

        private ApiResponser HandledInfo(feedBack value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var feedBackInfo = from fb in db.Feedbacks select fb;
                    Dictionary<int, Feedback> mapCheckBoxHandlling = new Dictionary<int, Feedback>();
                    if (feedBackInfo != null && feedBackInfo.Count() > 0)
                    {
                        foreach (var info in feedBackInfo)
                        {
                            if (info == null)
                            {
                                continue;
                            }
                            if (!mapCheckBoxHandlling.ContainsKey(info.FeedbackId))
                            {
                                mapCheckBoxHandlling.Add(info.FeedbackId, info);
                            }
                        }
                    }
                    if (value.CheckedBoxHandled != null && value.CheckedBoxHandled != "")
                    {
                        var feedbackHandled = value.CheckedBoxHandled;
                        string[] feedbackHandledStatus = feedbackHandled.Split(new Char[] { ',' });
                        for (var i = 0; i < feedbackHandledStatus.Length; i++)
                        {
                            if (feedbackHandledStatus[i] == "")
                            {
                                break;
                            }
                            var feedId = int.Parse(feedbackHandledStatus[i]);
                            if (mapCheckBoxHandlling.ContainsKey(feedId))
                            {
                                Feedback feedIdStatus = mapCheckBoxHandlling[feedId];
                                if (feedIdStatus.Status == 2 )
                                {
                                    continue;
                                }
                                else if (feedIdStatus.Status == 0 || feedIdStatus.Status == 1)
                                {
                                    feedIdStatus.Status = 2;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                    responser.Attach = new
                    {
                        HandledType = 0,
                    };

                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal("login failure", e);
                return responser;
            }
            return responser;
        }

        private ApiResponser SelectFeedBackDetail(feedBack value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var selectInfo = from f in db.Feedbacks
                                     join u in db.Users on f.UserId equals u.UserId
                                     where f.Status == value.StatusTypes
                                     select new
                                     {
                                         FeedbackId = f.FeedbackId,
                                         f.Content,
                                         year = f.CreatTime.Year,
                                         month = f.CreatTime.Month,
                                         days = f.CreatTime.Day,
                                         userName = u.RealName,
                                         Phone = u.Phone
                                     };
                    responser.Attach = new
                    {
                        SelectInfo = selectInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal("login failure", e);
                return responser;
            }
            return responser;
        }
        
        [System.Web.Http.HttpPost]
        public ApiResponser post(feedBack value)
        {
            if (value.recordType == FeedBack.sendFeedback)
            {
                return FeedbackInfo(value);
            }
            if (value.recordType == FeedBack.FeedbackDetail)
            {
                return FeedBackDetail(value);
            }
            if (value.recordType == FeedBack.handlling)
            {
                return Handlling(value);
            }
            if (value.recordType == FeedBack.handled)
            {
                return HandledInfo(value);
            }
            if (value.recordType == FeedBack.selectFeedback)
            {
                return SelectFeedBackDetail(value);
            }
            return null;
        }

	}
}