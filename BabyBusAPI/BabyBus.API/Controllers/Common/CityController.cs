using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BabyBus.Core.Helper;
using BabyBus.Service.Models;

namespace BabyBus.API.Controllers.Common
{
    public class CityController : ApiController
    {
        private static List<CityModel> cities = new List<CityModel>
        {
            new CityModel{CityId = 1,CityName = "西安"},
            new CityModel{CityId = 2,CityName = "咸阳"},
            new CityModel{CityId = 3,CityName = "宝鸡"},
            new CityModel{CityId = 4,CityName = "榆林"},
            new CityModel{CityId = 5,CityName = "延安"},
            new CityModel{CityId = 6,CityName = "汉中"},
            new CityModel{CityId = 7,CityName = "安康"},
        };

        public ApiResult<CityModel> Get()
        {
            var list = cities;
            return new ApiResult<CityModel>
            {
                Status = true,
                Items = list,
                TotalCount = list.Count
            };
        }
    }
}
