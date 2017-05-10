using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Service.Models.Enum
{
    public enum CheckoutType
    {
        RegisterDetail = 1,
        UploadImage,
        UpdateParentName,
        UpdateChildName,
        UpdateChildGender,
        UpdateChildBirthday,
        UpdateAll,
    }
}
