using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    public class PayRecordsController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(PayRecordsController));

        public enum PayRequestType
        {
            classMonths = 1,
            payChild =2,
            editPayType = 3,
        }
        public class PayRecords{
            public PayRequestType PayRequestType { get; set; }
            public int KindergartenId { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int ClassId { get; set; }
            public string CheckChildAlready { get; set; }
            public string CheckChildNo { get; set; }
        }

        private ApiResponser payRecordItemCls(PayRecords value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var payRecordItemClsCoutns = from cl in db.Classes
                                                 where cl.KindergartenId == value.KindergartenId 
                                                 && cl.ClassName != "删除"
                                                 && cl.Cancel == false
                                                 orderby cl.ClassType descending
                                                 select cl;
                    var payChildsInfo = from cp in db.ChildPays
                                        where cp.KindergartenId == value.KindergartenId
                                              && cp.CreatDate.Year == value.Year
                                              && cp.CreatDate.Month == value.Month
                                        group cp by cp.ClassId into gb
                                        select new
                                        {
                                            key = gb.Key,
                                            childsCount = from g in gb
                                                          where g.KindergartenId == value.KindergartenId
                                                          && g.CreatDate.Year == value.Year
                                                          && g.CreatDate.Month == value.Month
                                                          group g by g.PayType into pt
                                                          select new { key = pt.Key, childs = pt.Count() },
                                        };
                    responser.Attach = new
                    {
                        PayRecordItemClsCoutns = payRecordItemClsCoutns.ToList(),
                        PayChildsInfo = payChildsInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        private ApiResponser classChilds(PayRecords value)
        
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var childInfo = from ch in db.Child
                                    where ch.ClassId == value.ClassId
                                    select ch;
                    var payChildPayInfoType = from cp in db.ChildPays
                                              where cp.ClassId == value.ClassId && cp.CreatDate.Year == value.Year && cp.CreatDate.Month == value.Month
                                              select cp;
                    responser.Attach = new
                    {
                        ChildInfo = childInfo.ToList(),
                        PayChildPayInfoType = payChildPayInfoType.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }

        private ApiResponser editPayChildInfo(PayRecords value)
        {
            var dateTimes = System.DateTime.Now;
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    //1.查询
                    var payChildInfo = from pc in db.ChildPays
                                       where pc.CreatDate.Year == value.Year
                                             && pc.CreatDate.Month == value.Month
                                             && pc.ClassId == value.ClassId
                                       select pc;

                    //2Map
                    Dictionary<int, ChildPay> matchChildIdAndChildPayMap = new Dictionary<int, ChildPay>();
                    if (payChildInfo != null && payChildInfo.Count() != 0)
                    {
                        foreach (var payChild in payChildInfo)
                        {
                            if (payChild == null)
                            {
                                continue;
                            }
                            if (!matchChildIdAndChildPayMap.ContainsKey(payChild.ChildId))
                            {
                                matchChildIdAndChildPayMap.Add(payChild.ChildId, payChild);
                            }
                        }
                    }

                    //已缴费 payType = 1
                    if (value.CheckChildAlready != null && value.CheckChildAlready != "" && value.CheckChildAlready.Length > 0)
                    {
                        var payAlready = value.CheckChildAlready;
                        string[] payChildAready = payAlready.Split(new Char[] { ',' });
                        for (var i = 0; i < payChildAready.Length; i++)
                        {
                            var payChildId = payChildAready[i];
                            if (payChildId == "")
                            {
                                break;
                            }
                            var childId = int.Parse(payChildId);
                            if (matchChildIdAndChildPayMap.ContainsKey(childId))
                            {
                                ChildPay needUpdatePay = matchChildIdAndChildPayMap[childId];
                                if (needUpdatePay.PayType != 1)
                                {
                                    needUpdatePay.PayType = 1;
                                    needUpdatePay.PayDate = dateTimes;
                                }
                            }
                            else
                            {
                                ChildPay newPayChildInfo = new ChildPay
                                 {
                                     KindergartenId = value.KindergartenId,
                                     ClassId = value.ClassId,
                                     ChildId = childId,
                                     CreatDate = dateTimes,
                                     PayDate = dateTimes,
                                     PayType = 1,
                                 };
                                db.ChildPays.Add(newPayChildInfo);
                            }
                        }
                    }//已缴费结束
                    //2未缴费
                    if (value.CheckChildNo != null && value.CheckChildNo != "")
                    {
                        string[] payChildIdNo = value.CheckChildNo.Split(new Char[] { ',' });

                        for (var i = 0; i < payChildIdNo.Length; i++)
                        {
                            var payChildNo = payChildIdNo[i];
                            if (payChildNo == "")
                            {
                                break;
                            }
                            var childId2 = int.Parse(payChildNo);
                            if (matchChildIdAndChildPayMap.ContainsKey(childId2))
                            {
                                ChildPay needUpdatePay = matchChildIdAndChildPayMap[childId2];
                                if (needUpdatePay.PayType != 0)
                                {
                                    needUpdatePay.PayType = 0;
                                    needUpdatePay.PayDate = dateTimes;
                                }
                            }
                            else
                            {
                                ChildPay newPayChildInfo = new ChildPay
                                 {
                                     KindergartenId = value.KindergartenId,
                                     ClassId = value.ClassId,
                                     ChildId = childId2,
                                     CreatDate = dateTimes,
                                     PayDate = dateTimes,
                                     PayType = 0,
                                 };
                                db.ChildPays.Add(newPayChildInfo);
                            }
                        }//loop payChildIdNo end
                    }//未缴费结束
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        KK = 0,
                    };
                }//using end
            }
            catch (Exception e)
            {
                responser.Status = false;
                responser.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responser;
            }
            return responser;
        }
        //
        // GET: /PayRecords/
        [System.Web.Http.HttpPost]
        public ApiResponser post(PayRecords value)
        {
            if(value.PayRequestType == PayRequestType.classMonths)
            {  
                return payRecordItemCls(value);
            }
            else if (value.PayRequestType == PayRequestType.payChild)
            {
                return classChilds(value);
            }else if(value.PayRequestType == PayRequestType.editPayType)
            {
                return editPayChildInfo(value);
            }
            return null;
        }
	}
}