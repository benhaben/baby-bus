using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Service.General;
using BabyBus.Service.General.Attendance;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Attendance {
    public class AttMasterController : ApiController {
        private readonly IAttendanceService _attService;
        private readonly IChildService _childService;
        private readonly IClassService _classService;
        private readonly IParentChildRelationService _pcrService;

        public AttMasterController(IAttendanceService attService, IClassService classService,
            IChildService childService, IParentChildRelationService pcrService) {
            _attService = attService;
            _classService = classService;
            _childService = childService;
            _pcrService = pcrService;
        }

        //Get:api/AttMaster
        /// <summary>
        ///     Get Attendance List By Kindergarten(Or Class)
        /// </summary>
        public ApiResult<AttendanceModel> Get(DateTimeOffset date, int kId, int classId = 0) {
            var result = new ApiResult<AttendanceModel>();
            var prelist = from a in _attService.GetAllMaster()
                where a.KindergartenId == kId && (classId == 0 || a.ClassId == classId) && a.CreateDate == date
                select new {
                    a.MasterId,
                    a.ClassId,
                    a.Total,
                    a.Attence,
                    a.CreateDate
                };

            IEnumerable<AttendanceModel> list = from c in _classService.GetAllClass()
                join a in prelist on c.ClassId equals a.ClassId into joinedAtt
                from att in joinedAtt.DefaultIfEmpty()
                where c.KindergartenId == kId && (classId == 0 || c.ClassId == classId)
                select new AttendanceModel {
                    MasterId = att != null ? att.MasterId : 0,
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    Total = att != null ? att.Total : c.ClassCount,
                    Attence = att != null ? att.Attence : 0,
                    CreateDate = date,
                };
            List<AttendanceModel> enumerable = list.ToList();

            result.Status = true;
            result.Items = enumerable;
            result.TotalCount = enumerable.Count;
            return result;
        }

        //Post:api/AttMaster
        /// <summary>
        ///     Attence By Class and Date
        /// </summary>
        public ApiResponser Post(AttendanceModel model) {
            var response = new ApiResponser(true);

            try {
                _attService.WorkAttendance(model);

                //Push
                List<int> children = (from c in _childService.GetAllChild()
                    where c.ClassId == model.ClassId
                    select c.ChildId).ToList();
                List<int> preChilren = (from a in _attService.GetAllDetail()
                    where a.MasterId == model.MasterId
                    select a.ChildId).ToList();
                IEnumerable<int> attChildren = model.AttChildren.Except(preChilren);
                var unattChildren = new List<int>();
                if (preChilren.Count == 0) {
                    unattChildren = children.Except(model.AttChildren).ToList();
                }
                string[] attUserList = (from pcr in _pcrService.GetAllParentChildRelation()
                    join c in attChildren on pcr.ChildId equals c
                    select pcr.UserId.ToString()).ToArray();
                string[] unattUserList = (from pcr in _pcrService.GetAllParentChildRelation()
                    join c in unattChildren on pcr.ChildId equals c
                    select pcr.UserId.ToString()).ToArray();
                JPushUtils.PushAttendance(attUserList, unattUserList);
            }
            catch (Exception ex) {
            }
            return response;
        }
    }
}