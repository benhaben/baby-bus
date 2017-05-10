using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBus.Models.Enums;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Attributes;

namespace BabyBus.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        
        [PrimaryKey]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int WeChatId { get; set; }
        public string RealName { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public bool HasLogin { get; set; }
        public RoleType RoleType { get; set; }

        public string ImageName { get; set; }
        public int CityId { get; set; }

        /// <summary>
        /// 对老师，园长有用
        /// </summary>
        public int KindergartenId { get; set; }

        public int ClassId { get; set; }

        public int ChildId { get; set; }
        [Ignore]
        public CityModel City { get; set; }
        [Ignore]
        public KindergartenModel Kindergarten { get; set; }
        [Ignore]
        public KindergartenClassModel Class { get; set; }
        [Ignore]
        public ChildModel Child { get; set; }
        [Ignore]
        public bool IsCheckout { get; set; }
        [Ignore]
        public AuditType CheckoutAuditType { get; set; }
        [Ignore]
        public string CheckoutMemo { get; set; }
    }
}
