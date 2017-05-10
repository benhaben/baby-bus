using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.Models
{
    /// <summary>
    /// User Detail Info, Including Child, Kindergarten and Class
    /// </summary>
    public class UserDetailModel : User
    {
        public UserDetailModel() { }       
        public UserDetailModel(User basic)
        {
            Mapper.DynamicMap(basic, this);
        }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public int ChildId { get; set; }

        public Child Child { get; set; }
        public Kindergarten Kindergarten { get; set; }
        public Class Class { get; set; }
        public bool IsCheckout { get; set; }
        public AuditType CheckoutAuditType { get; set; }
        public string CheckoutMemo { get; set; }
    }
}
