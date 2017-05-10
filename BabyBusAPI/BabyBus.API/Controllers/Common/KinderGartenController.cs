using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Query;
using AutoMapper;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.General;
using BabyBus.Service.General.Main;

namespace BabyBus.API.Controllers.Common
{
    public class KinderGartenController : ApiController
    {
        private readonly IKindergartenService _service;

        public KinderGartenController(IKindergartenService service)
        {
            _service = service;
        }

        // POST: api/KinderGarten add & edit
        public ApiResponser Post(Kindergarten kindergarten)
        {
            var response = new ApiResponser(true);
            if (kindergarten.KindergartenId != 0)
            {
                _service.EditKindergarten(kindergarten);
                response.Attach = kindergarten;
            }
            else
            {
                _service.CreateKindergarten(kindergarten);
                response.Attach = kindergarten;
            }
            return response;
        }


        // DELECT: api/kindergarten delete
        public ApiResponser Delete(int id)
        {
            var response = new ApiResponser(true);
            var result = _service.GetKindergarten(id);
            if (result != null)
            {
                _service.DeleteKindergarten(id);
                response.Attach = result;
            }
            return response;
        }

        // GET: api/KinderGarten inquery
        public ApiResult<Kindergarten> Get(ODataQueryOptions<Kindergarten> options)
        {
            var response = new ApiResult<Kindergarten>();
            var list = _service.GetAllKindergarten().AsQueryable();
            var result = options.ApplyTo(list) as IEnumerable<Kindergarten>;

            if (result != null)
            {
                var enumerable = result.ToList();
                response.Status = true;
                response.Items = enumerable;
                response.TotalCount = enumerable.Count;
            }
            else
            {
                response.Status = false;
                response.Message = "查询期间发生错误";
            }
            return response;

        }
    }
}