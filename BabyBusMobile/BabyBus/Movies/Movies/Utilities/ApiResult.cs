using System;
using System.Collections.Generic;
using BabyBus.Models;

namespace BabyBus.Services
{
    /// <summary>
    ///     查询返回的Api对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        public ApiResult()
        {
            Status = true;
        }

        public bool Status { get; set; }
        public string Message { get; set; }
        public List<T> Items { get; set; }
        public Uri NextPageLink { get; set; }
        public long TotalCount { get; set; }
    }

	public class ApiResult1
	{
		public ApiResult1()
		{
			Status = true;
		}

		public bool Status { get; set; }
		public string Message { get; set; }
		public List<CityModel> Items { get; set; }
		public Uri NextPageLink { get; set; }
		public Nullable<long> TotalCount { get; set; }
	}
}