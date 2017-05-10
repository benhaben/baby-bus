using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;

namespace AdministratorManagement.Controllers
{
    public class GiftedYoungController : ApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(GiftedYoungController));

        public enum GiftedType
        {
            giftedYoung = 0,
            giftedItemDetail = 1,
            sendGiftedYound = 2
        }

        public class GiftedYoung
        {
            public GiftedType giftedType { get; set; }
            public int KindergartenId { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int NoticeId { get; set; }
            public string Kindergarten { get; set; }
            public int ClassId { get; set; }
            public int UserId { get; set; }
            public int NoticeType { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string ThumPics { get; set; }
            public string NormalPics { get; set; }
            public int ReceiverNumber { get; set; }
            public int FavoriteCount { get; set; }
            public int ReadedCount { get; set; }
            public int ConfirmedCount { get; set; }
        }

        private ApiResponser SendGiftedYoungInfo(GiftedYoung value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.Month == 0)
                    {
                        var giftedYoungInfo = from n in db.Notice
                            where
                                (n.KindergartenId == 0 || n.KindergartenId == value.KindergartenId) && n.NoticeType == 8 && n.CreateTime.Year == value.Year
                            select new
                            {
                                Titles = n.Title,
                                Months = n.CreateTime.Month,
                                NoticeIds = n.NoticeId,
                                Days = n.CreateTime.Day,
                                kinderId = n.KindergartenId,
                            };
                        responser.Attach = new
                        {
                            GiftedYoungDetail = giftedYoungInfo.ToList(),
                        };
                    }
                    else
                    {
                        var giftedInfo = from n in db.Notice
                            where
                                n.KindergartenId == value.KindergartenId && n.CreateTime.Year == value.Year &&
                                n.CreateTime.Month == value.Month && n.NoticeType == 8
                                         select new
                                         {
                                             Titles = n.Title,
                                             Months = n.CreateTime.Month,
                                             NoticeIds = n.NoticeId,
                                             Days = n.CreateTime.Day,
                                             types = n.NoticeType,
                                             kinderId = n.KindergartenId,
                                         };
                        responser.Attach = new
                        {
                            GiftedYoungDetail = giftedInfo.ToList(),
                        };
                    }
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

        private ApiResponser GiftedYoundDetail(GiftedYoung value)
        {
            var responer = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var giftedInfo = from n in db.Notice
                                     where n.NoticeId == value.NoticeId
                                     select new
                                     {
                                         n,
                                         years = n.CreateTime.Year,
                                         months = n.CreateTime.Month,
                                         days = n.CreateTime.Day,
                                     };
                    responer.Attach = new
                    {
                        GiftedDetail = giftedInfo.ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                responer.Status = false;
                responer.Message = e.Message;
                Log.Fatal(e.Message, e);
                return responer;
            }
            return responer;
        }

        private ApiResponser SubmitGiftedYound(GiftedYoung value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    var kinderVal = value.Kindergarten;
                    string[] kinderIdInfo = kinderVal.Split(new Char[] {','});
                    for (var i = 0; i < kinderIdInfo.Length; i++)
                    {
                        if (kinderIdInfo[i] == "")
                        {
                            continue;
                        }
                        var kinderId = int.Parse(kinderIdInfo[i]);
                        var newGiftedYount = new Notice
                        {
                             KindergartenId = kinderId,
                            ClassId = value.ClassId,
                            UserId = value.UserId,
                            NoticeType = value.NoticeType,
                            Title = value.Title,
                            Content = value.Content,
                            ThumPics =  null,
                            NormalPics = value.NormalPics,
                            ReceiverNumber = value.ReceiverNumber,
                            FavoriteCount = value.FavoriteCount,
                            ReadedCount = value.ReadedCount,
                            ConfirmedCount = value.ConfirmedCount
                        };
                        db.Notice.Add(newGiftedYount);
                    }
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        status = "success"
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
        // GET: /GiftedYoung/
        [System.Web.Http.HttpPost]
        public ApiResponser post(GiftedYoung value)
        {
            if (value.giftedType == GiftedType.giftedYoung)
            {
                return SendGiftedYoungInfo(value);
            }
            if (value.giftedType == GiftedType.giftedItemDetail)
            {
                return GiftedYoundDetail(value);
            }
            if (value.giftedType == GiftedType.sendGiftedYound)
            {
                return SubmitGiftedYound(value);
            }
            return null;
        }
	}
}