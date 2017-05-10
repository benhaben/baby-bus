using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.General;
using BabyBus.Service.General.Main;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Children {
    public class ChildController : ApiController {
        private readonly IParentChildRelationService _pcrService;
        private readonly IChildService _service;
        private readonly IUserService _userService;

        public ChildController(IChildService service, IParentChildRelationService pcrService, IUserService userService) {
            _service = service;
            _pcrService = pcrService;
            _userService = userService;
        }

        //Post: api/Child add & edit
        public ApiResponser Post(Child child) {
            var response = new ApiResponser(true);
            if (child.ChildId != 0) {
                _service.EditChild(child);
                response.Attach = child;
            }
            else {
                _service.CreateChild(child);
                response.Attach = child;
            }
            return response;
        }

        //Get: api/Child
        public ApiResult<ChildModel> GetChild(ODataQueryOptions<Child> options) {
            var response = new ApiResult<ChildModel>();
            IQueryable<Child> list = _service.GetAllChild();

            list = options.ApplyTo(list) as IQueryable<Child>;
            IQueryable<ChildModel> result = from c in list
                join pcr in _pcrService.GetAllParentChildRelation() on c.ChildId equals pcr.ChildId
                join u in _userService.GetAllUser() on pcr.UserId equals u.UserId into user
                select new ChildModel {
                    ChildId = c.ChildId,
                    KindergartenId = c.KindergartenId,
                    ClassId = c.ClassId,
                    CardPasswordId = c.CardPasswordId,
                    ChildName = c.ChildName,
                    Gender = c.Gender,
                    CreateTime = c.CreateTime,
                    ImageName = c.ImageName,
                    Cancel = c.Cancel,
                    Birthday = c.Birthday,
                    ParentName = user.FirstOrDefault().RealName,
                    ParentPhone = user.FirstOrDefault().LoginName
                };
            var enumerable = new List<ChildModel>();
            var temp = result.ToList();
            //Remove Duplicated Child
            foreach (var child in temp) {
                if (enumerable.All(x => x.ChildId != child.ChildId)) {
                    enumerable.Add(child);
                }
            }
            response.Status = true;
            response.Items = enumerable;
            response.TotalCount = enumerable.Count;
            return response;
        }
    }
}