using System;
using System.Configuration;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.Models.Enum;

namespace BabyBus.Service.Models
{
    public class CheckoutModel : Checkout
    {
        public CheckoutModel() {
            HasHeadImage = false;
            RealName = Constant.StringNull;
            ChildName = Constant.StringNull;
            Gender = Constant.IntNull;
            Birthday = Constant.DateNull;
        }

        public CheckoutType CheckoutType { get; set; }

        public bool HasHeadImage { get; set; }

        public string LoginName { get; set; }
        public string ClassName { get; set; }
    }
}