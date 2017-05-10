using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Core.Helper
{
    public class ApiResult<T>
    {

        public bool Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Items { get; set; }
        public Uri NextPageLink { get; set; }
        public Nullable<long> TotalCount { get; set; }
    }
}
