using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Core.Helper
{
    public class ApiResponser
    {
        public ApiResponser() { }

        public ApiResponser(bool status)
        {
            Status = status;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 其他信息
        /// </summary>
        public object Attach { get; set; }
    }
}
