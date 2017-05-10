using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Login.Enum
{
    /// <summary>
    /// 审批类型
    /// </summary>
    public enum AuditType
    {
        Pending = 1,
        Passed = 2,
        Refused = 3
    }
}
