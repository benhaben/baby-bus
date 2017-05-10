using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Service.General;
using BabyBus.Service.General.Attendance;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;
using BabyBus.Service.Models.Enum;

namespace BabyBus.API.Controllers.Attendance {
    public class AttDetailController : ApiController {
        private readonly IAttendanceService _attService;
        private readonly IChildService _childService;

        public AttDetailController(IChildService childService, IAttendanceService attService) {
            _childService = childService;
            _attService = attService;
        }

        //Get:Api/AttDetail
        /// <summary>
        ///     Get Attendance Detail By Class and Date
        /// </summary>
        public ApiResult<ChildModel> Get(int classId, DateTimeOffset date, AttenceType type = AttenceType.All) {
            var result = new ApiResult<ChildModel>();

            var prelist = from a in _attService.GetAllDetail()
                join b in _attService.GetAllMaster() on a.MasterId equals b.MasterId
                where b.ClassId == classId && a.CreateDate == date
                select new {a.ChildId};

            IQueryable<ChildModel> list = from c in _childService.GetAllChild()
                join a in prelist on c.ChildId equals a.ChildId into joinedAtt
                from a in joinedAtt.DefaultIfEmpty()
                where c.ClassId == classId
                select new ChildModel {
                    ChildId = c.ChildId,
                    ChildName = c.ChildName,
                    ImageName = c.ImageName,
                    IsSelect = a != null,
                };

            List<ChildModel> enumerable = list.ToList();

            switch (type) {
                case AttenceType.Attence:
                    enumerable = enumerable.Where(x => x.IsSelect).ToList();
                    break;
                case AttenceType.UnAttence:
                    enumerable = enumerable.Where(x => x.IsSelect == false).ToList();
                    break;
            }

            result.Status = true;
            result.Items = enumerable;
            result.TotalCount = enumerable.Count;
            return result;
        }
    }
}