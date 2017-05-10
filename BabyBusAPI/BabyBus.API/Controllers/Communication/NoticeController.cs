using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.OData.Query;
using AutoMapper;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Article;
using BabyBus.Model.Entities.Article.Enum;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Service.General;
using BabyBus.Service.General.Communication;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Communication
{
    public class NoticeController : ApiController
    {
        private readonly IChildService _childService;
        private readonly INoticeService _noticeService;
        private readonly IUserService _userService;

        public NoticeController(INoticeService s, IUserService u, IChildService c) {
            _noticeService = s;
            _userService = u;
            _childService = c;
        }

        //Post: api/NoticeHomework
        public ApiResponser Post(NoticeModel model) {
            var response = new ApiResponser(true);
            model.CreateTime = DateTime.Now;
            try
            {
                GetImageName(model);
                var notice = new Notice();
                Mapper.DynamicMap(model, notice);
                model.NoticeId = _noticeService.CreateNotice(notice);
                response.Attach = model;
                model.Base64ImageList = null; //clear base64
                //Push
                JPushUtils.PushMessage(notice.NoticeType, notice.Title, model.KindergartenId, model.ClassId, model.NoticeId);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        //Get: api/NoticeHomework
        public ApiResult<Notice> Get(RoleType type, int id,NoticeViewType viewType, ODataQueryOptions<Notice> options) {
            var ar = new ApiResult<Notice>();
            IQueryable<Notice> prelist = _noticeService.GetAllNotice();
            IQueryable<Notice> list;
            IQueryable<NoticeModel> result;

            if (type == RoleType.Teacher) {
                list = (from n in prelist
                    join u in _userService.GetAllUser() on new {n.KindergartenId, n.ClassId} equals
                        new {u.KindergartenId, u.ClassId}
                    where u.UserId == id
                          && (((viewType == NoticeViewType.Notice && n.NoticeType != NoticeType.GrowMemory))
                              || (viewType == NoticeViewType.GrowMemory && n.NoticeType == NoticeType.GrowMemory))
                    select n);
                if (viewType == NoticeViewType.NoticeOnlyKg || viewType == NoticeViewType.Notice) {
                    list = list.Union(from n in prelist
                        join u in _userService.GetAllUser() on n.KindergartenId equals u.KindergartenId
                        where u.UserId == id
                              &&
                              (n.NoticeType == NoticeType.KindergartenAll ||
                               n.NoticeType == NoticeType.KindergartenStaff)
                        select n).Distinct();
                }

                result = from n in options.ApplyTo(list) as IQueryable<Notice>
                    join u in _userService.GetAllUser() on n.UserId equals u.UserId
                    select new NoticeModel
                    {
                        NoticeId = n.NoticeId,
                        KindergartenId = n.KindergartenId,
                        ClassId = n.ClassId,
                        UserId = n.UserId,
                        NoticeType = n.NoticeType,
                        Title = n.Title,
                        Content = n.Content,
                        CreateTime = n.CreateTime,
                        NormalPics = n.NormalPics,
                        ReceiverNumber = n.ReceiverNumber,
                        FavoriteCount = n.FavoriteCount,
                        ReadedCount = n.ReadedCount,
                        ConfirmedCount = n.ConfirmedCount,
                        RealName = u.RealName
                    };
            }
            else if (type == RoleType.HeadMaster) {
                list = (from n in prelist
                    join u in _userService.GetAllUser() on n.KindergartenId equals u.KindergartenId
                    where u.UserId == id
                          &&
                          (n.NoticeType == NoticeType.KindergartenAll || n.NoticeType == NoticeType.KindergartenStaff)
                    select n).Distinct();

                result = from n in options.ApplyTo(list) as IQueryable<Notice>
                    join u in _userService.GetAllUser() on n.UserId equals u.UserId
                    select new NoticeModel
                    {
                        NoticeId = n.NoticeId,
                        KindergartenId = n.KindergartenId,
                        ClassId = n.ClassId,
                        UserId = n.UserId,
                        NoticeType = n.NoticeType,
                        Title = n.Title,
                        Content = n.Content,
                        CreateTime = n.CreateTime,
                        NormalPics = n.NormalPics,
                        ReceiverNumber = n.ReceiverNumber,
                        FavoriteCount = n.FavoriteCount,
                        ReadedCount = n.ReadedCount,
                        ConfirmedCount = n.ConfirmedCount,
                        RealName = u.RealName
                    };
            }
            else
            {
                list = (from n in prelist
                    join c in _childService.GetAllChild() on new {n.KindergartenId, n.ClassId} equals
                        new {c.KindergartenId, c.ClassId}
                    where c.ChildId == id
                               && (((viewType == NoticeViewType.Notice && n.NoticeType != NoticeType.GrowMemory))
                               || (viewType == NoticeViewType.GrowMemory && n.NoticeType == NoticeType.GrowMemory))
                    select n);
                if (viewType == NoticeViewType.NoticeOnlyKg || viewType == NoticeViewType.Notice) {
                    list = list.Union(from n in prelist
                               join c in _childService.GetAllChild() on n.KindergartenId equals c.KindergartenId
                               where c.ChildId == id
                                         && n.NoticeType == NoticeType.KindergartenAll
                               select n).Distinct();
                }

                result = from n in options.ApplyTo(list) as IQueryable<Notice>
                    join u in _userService.GetAllUser() on n.UserId equals u.UserId
                    select new NoticeModel
                    {
                        NoticeId = n.NoticeId,
                        KindergartenId = n.KindergartenId,
                        ClassId = n.ClassId,
                        UserId = n.UserId,
                        NoticeType = n.NoticeType,
                        Title = n.Title,
                        Content = n.Content,
                        CreateTime = n.CreateTime,
                        NormalPics = n.NormalPics,
                        ReceiverNumber = n.ReceiverNumber,
                        FavoriteCount = n.FavoriteCount,
                        ReadedCount = n.ReadedCount,
                        ConfirmedCount = n.ConfirmedCount,
                        RealName = u.RealName
                    };
            }
            List<NoticeModel> enumerable = result.ToList();
            ar.Status = true;
            ar.Items = enumerable;
            ar.TotalCount = enumerable.Count;
            return ar;
        }

        private void GetImageName(NoticeModel model) {
            if (model.ImageCount > 0)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < model.ImageCount; i++)
                {
                    var guid = Guid.NewGuid();
                    var fileName = guid + Config.PhotoSuffix;
                    sb.Append(fileName + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                model.NormalPics = sb.ToString();
            }
            else
            {
                model.NormalPics = string.Empty;
            }
        }
    }
}