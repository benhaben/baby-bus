using AdministratorManagement.Core;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdministratorManagement.Controllers
{
    [Authorize]
    public class UploadAdvertisementPicController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(UploadAdvertisementPicController));

        public enum advType
        {
            uploadAdv = 1,
            detailAdv = 2,
            submitCheck = 3,
        }
        public class UploadAdvertisement
        {
            public advType advType { get; set; }
            public string NomalsPics { get; set; }
            public int UserId { get; set; }
            public string Description { get; set; }
            public string UserType { get; set; }
            public Nullable<System.Guid> Guid { get; set; }
            public string CheckedTrue { get; set; }
            public string CheckedFalse { get; set; }

        }

        private ApiResponser UploadAdvertisementPicDetail(UploadAdvertisement value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    /*var uploadPicTime = DateTime.Now;

                    var lastYear = uploadPicTime.Year - 1;
                    List<int> lastMonth = new List<int>();
                    List<int> currentYearMonth = new List<int>();

                    if (uploadPicTime.Month == 1)
                    {
                        lastMonth.Add(12);
                        lastMonth.Add(11);
                        currentYearMonth.Add(1);
                    }
                    else if (uploadPicTime.Month == 2)
                    {
                        lastMonth.Add(12);
                        currentYearMonth.Add(1);
                        currentYearMonth.Add(2);
                    }



                    if (uploadPicTime.Month == 1 || uploadPicTime.Month == 2)
                    {
                        var advertisementDetai1l = from ad in db.AdvertisementSets
                                                  where (ad.CreateTime.Year == lastYear
                                                  && lastMonth.Contains(ad.CreateTime.Month)
                                                  )
                                                  ||
                                                  (ad.CreateTime.Year == uploadPicTime.Year
                                                  && currentYearMonth.Contains(ad.CreateTime.Month))
                                                  orderby ad.CreateTime
                                                  select ad;

                        /*if (uploadPicTime.Month == 1)
                        {
                            var advertisementDetail = from ad in db.AdvertisementSets
                                                      where (ad.CreateTime.Year == uploadPicTime.Year && (ad.CreateTime.Month == uploadPicTime.Month)) ||
                                                      (ad.CreateTime.Year == uploadPicTime.Year - 1 && (ad.CreateTime.Month == 11 || ad.CreateTime.Month == 12))
                                                      orderby ad.CreateTime
                                                      select ad;
                            responser.Attach = new
                            {
                                AdvertisementDetailInfo = advertisementDetail.ToList(),
                            };
                        }
                        else
                        {
                            var advertisementDetail = from ad in db.AdvertisementSets
                                                      where (ad.CreateTime.Year == uploadPicTime.Year-1 && (ad.CreateTime.Month == 12)) ||
                                                      (ad.CreateTime.Year == uploadPicTime.Year  && (ad.CreateTime.Month == uploadPicTime.Month || ad.CreateTime.Month == uploadPicTime.Month -1))
                                                      orderby ad.CreateTime
                                                      select ad;
                            responser.Attach = new
                            {
                                AdvertisementDetailInfo = advertisementDetail.ToList(),
                            };
                        }
                    }else{*/
                    var advertisementDetail = from ad in db.AdvertisementSets
                                              orderby ad.CreateTime
                                              select new
                                              {
                                                  AderId = ad.AderId,
                                                  NormalPics = ad.NormalPics,
                                                  Description = ad.Description,
                                                  Year = ad.CreateTime.Year,
                                                  Month = ad.CreateTime.Month,
                                                  Day = ad.CreateTime.Day,
                                                  IsUsed = ad.IsUsed
                                              };
                        responser.Attach = new
                        {
                            AdvertisementDetailInfo = advertisementDetail.ToList(),
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
        private ApiResponser UploadPic(UploadAdvertisement value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities()) {
                    var newAdverPic = new AdvertisementSet
                    {
                        CreateTime = DateTime.Now,
                        NormalPics = value.NomalsPics,
                        IsUsed = 0,
                        UserId = value.UserId,
                        Description = value.Description,
                        UserType = value.UserType,
                        Guid = value.Guid,
                    };
                    db.AdvertisementSets.Add(newAdverPic);
                    db.SaveChanges();
                    responser.Attach = new
                    {
                        UploadType = 0,
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


        private ApiResponser SubmitCheckPic(UploadAdvertisement value)
        {
            var responser = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    /*var uploadPicTime = DateTime.Now;

                    var lastYear = uploadPicTime.Year - 1;
                    List<int> lastMonth = new List<int>();
                    List<int> currentYearMonth = new List<int>();

                    if (uploadPicTime.Month == 1)
                    {
                        lastMonth.Add(12);
                        lastMonth.Add(11);
                        currentYearMonth.Add(1);
                    }
                    else if (uploadPicTime.Month == 2)
                    {
                        lastMonth.Add(12);
                        currentYearMonth.Add(1);
                        currentYearMonth.Add(2);
                    }


                    if(DateTime.Now.Month == 1 || DateTime.Now.Month == 2){
                        var advertisementDetai1l = from ad in db.AdvertisementSets
                                                  where (ad.CreateTime.Year == lastYear
                                                  && lastMonth.Contains(ad.CreateTime.Month)
                                                  )
                                                  ||
                                                  (ad.CreateTime.Year == uploadPicTime.Year
                                                  && currentYearMonth.Contains(ad.CreateTime.Month))
                                                  orderby ad.CreateTime
                                                  select ad;
                        Dictionary<int, AdvertisementSet> selectPicMap = new Dictionary<int,AdvertisementSet>();
                        if(advertisementDetai1l != null && advertisementDetai1l.Count() != 0){
                            foreach(var advPic in advertisementDetai1l){
                                if(advPic == null){
                                    continue;
                                }
                                if(!selectPicMap.ContainsKey(advPic.AderId)){
                                    selectPicMap.Add(advPic.AderId, advPic);
                                }
                            }

                        }
                    }else {*/
                        var advertisementDetail = from ad in db.AdvertisementSets
                                                  orderby ad.CreateTime
                                                  select ad;
                        var dddd = advertisementDetail.Count();
                        Dictionary<int, AdvertisementSet> selectPicMap = new Dictionary<int, AdvertisementSet>();
                        if (advertisementDetail != null && advertisementDetail.Count() != 0)
                        {
                            foreach (var advPic in advertisementDetail)
                            {
                                if (advPic == null)
                                {
                                    continue;
                                }
                                if (!selectPicMap.ContainsKey(advPic.AderId))
                                {
                                    selectPicMap.Add(advPic.AderId, advPic);
                                }
                            }

                        }
                    if(value.CheckedTrue != null && value.CheckedTrue != ""){
                        var selectPicInfo = value.CheckedTrue;
                        string[] itemPicInfoArry = selectPicInfo.Split(new Char[] { ',' });
                        for (var i = 0; i < itemPicInfoArry.Length; i++)
                        {
                            var picAdv = itemPicInfoArry[i];
                            if (picAdv == "")
                            {
                                break;
                            }
                            var picAdvIdVal = int.Parse(picAdv);
                            if (selectPicMap.ContainsKey(picAdvIdVal))
                            {
                                AdvertisementSet updateAdvPic = selectPicMap[picAdvIdVal];
                                updateAdvPic.IsUsed = 1;
                            }
                        }
                        db.SaveChanges();
                    }
                    if (value.CheckedFalse != null && value.CheckedFalse != "")
                    {
                        var selectPicInfoFalse = value.CheckedFalse;
                        string[] itemPicInfoFalseArry = selectPicInfoFalse.Split(new Char[] { ',' });
                        for (var i= 0; i < itemPicInfoFalseArry.Length; i++)
                        {
                            var picAdvFalse = itemPicInfoFalseArry[i];
                            if(picAdvFalse == ""){
                                break;
                            }
                            var picAdvIdFalse = int.Parse(picAdvFalse);
                            if (selectPicMap.ContainsKey(picAdvIdFalse))
                            {
                                AdvertisementSet updateAdvPicFalse = selectPicMap[picAdvIdFalse];
                                updateAdvPicFalse.IsUsed = 0;
                            }
                        }
                        db.SaveChanges();
                    }
                    responser.Attach = new
                    {
                        Updatasuccess = 0,
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
        [System.Web.Http.HttpPost]
        public ApiResponser Post(UploadAdvertisement value)
        
        {
            if (value.advType == advType.uploadAdv)
            {
                return UploadPic(value);
            }
            if (value.advType == advType.detailAdv)
            {
                return UploadAdvertisementPicDetail(value);
            }
            if (value.advType == advType.submitCheck)
            {
                return SubmitCheckPic(value);
            }
            return null;
        }
    }
}