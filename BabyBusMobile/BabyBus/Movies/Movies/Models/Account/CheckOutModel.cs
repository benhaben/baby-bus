using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models.Enums;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Models.Account
{
    public class CheckoutModel : MvxNotifyPropertyChanged
    {
        public CheckoutModel()
        {
            RoleType = RoleType.Parent;
            HasHeadImage = false;
            RealName = Constants.StringNull;
            ChildName = Constants.StringNull;
            Gender = Constants.IntNull;
            Birthday = Constants.DateNull;
        }

        public int CheckoutId { get; set; }
        public CheckoutType CheckoutType { get; set; }
        public RoleType RoleType { get; set; }
        public int UserId { get; set; }
        public string RealName { get; set; }
        public string VerifyCode { get; set; }
        public string City { get; set; }
        public int KindergartenId { get; set; }
        public int ClassId { get; set; }
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateTime { get; set; }
        public string Base64Image { get; set; }
        public string ImageName { get; set; }

        [Ignore]
        public bool HasHeadImage { get; set; }

        private AuditType _auditType = AuditType.Pending;

        public AuditType AuditType {
            get { return _auditType; }
            set {
                _auditType = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public string Memo { get; set; }

        public string Status {
            get {
                if (AuditType == AuditType.Pending)
                {
                    return "待审批";
                }
                else if (AuditType == AuditType.Refused)
                {
                    return "未通过";
                }
                else
                {
                    return "已通过";
                }
                
            }
        }

        public string GenderName {
            get {
                return Gender == 1 ? "男" : "女";
            }
        }

        public string LoginName { get; set; }
    }
}
