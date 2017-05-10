using System;
using System.Collections.Generic;
using BabyBus.Models;

namespace BabyBus {
    public static class Constants {
        #region Base

        //TODO: define DEBUGAPI in the adhoc, but adhoc can not work currently
        #if DEBUG
//                public const string BaseUrl = "http://115.28.2.41/Api";
        public const string BaseUrl = "http://imreliable.net/api";
        



#else
//          public const string BaseUrl = "http://115.28.2.41/Api";
        public const string BaseUrl = "http://imreliable.net/api";
        #endif

        #endregion

        #region OData

        public const string ODataTop = "$top={0}";
        public const string ODataSkip = "$skip={0}";

        #endregion

        #region Common

        public const string Kindergartens = BaseUrl + "/Kindergarten?$filter=City eq '{0}'";
        public const string Cities = BaseUrl + "/City";
        public const string Classes = BaseUrl + "/Class?$filter=KindergartenId eq {0}";
        public const string Versions = BaseUrl + "/Version?appType={0}";
        public const string ChildSummary = BaseUrl + "/ChildSummary?childId={0}";
        #endregion

        #region Member

        public const string UserInfoByLoginName = "User?$filter=LoginName eq '{0}'";

        public const string Login = BaseUrl + "/Login";

        public const string Register = BaseUrl + "/Register";
        public const string UpdateUserInfo = BaseUrl + "/RegisterDetail";
        public const string Checkout = BaseUrl + "/Checkout";
        public const string CheckoutList = BaseUrl + "/Checkout?$filter=KindergartenId eq {0} and ClassId eq {1}";
        public const string ChangePassword = BaseUrl + "/Password";

        #endregion

        #region Test

        public const string TestRecord = "TestRecord?$filter=ChildID eq {0}";

        #endregion

        #region Communication

        public const string SendNotice = BaseUrl + "/Notice";
        public const string NewNotices = BaseUrl + "/Notice?type={0}&id={1}&viewType={2}&$filter=NoticeId gt {3}&$orderby=CreateTime desc";
        public const string TopNewNotices = BaseUrl + "/Notice?type={0}&id={1}&viewType={2}&$top={3}&$orderby=CreateTime desc";
        public const string TopOldNotices = BaseUrl + "/Notice?type={0}&id={1}&viewType={2}&$top={3}&$filter=NoticeId lt {4}&$orderby=CreateTime desc";
        public const string SendQuestion = BaseUrl + "/Question";
        public const string NewQuestions = BaseUrl + "/Question?type={0}&id={1}&$filter=QuestionId gt {2}&$orderby=CreateTime desc";
        public const string TopNewQuestions = BaseUrl + "/Question?type={0}&id={1}&$top={2}&$orderby=CreateTime desc";
        public const string TopOldQuestions = BaseUrl + "/Question?type={0}&id={1}&$top={2}&$filter=QuestionId lt {3}&$orderby=CreateTime desc";
        public const string SendAnswer = BaseUrl + "/Answer";

        #endregion

        #region Attendance

        public const string GetAttendanceMasterList = BaseUrl + "/AttMaster?kId={0}&classId={1}&date={2}";
        public const string GetAttendanceChildList = BaseUrl + "/AttDetail?classId={0}&date={1}&type={2}";
        public const string WorkAttendance = BaseUrl + "/AttMaster";
        public const string GetChildMonthAttendance = BaseUrl + "/ChildMonthAtt?childId={0}&date={1}";
        #endregion

        #region Setting

        public const string GetChildrenByClassId = BaseUrl + "/Child?$filter=ClassId eq {0}";

        #endregion

        #region Null Value

        public const int IntNull = -999;
        public static readonly DateTime DateNull = DateTime.MinValue;
        public const string StringNull = null;

        #endregion

        #region Permission 暂时把权限硬编码，后面再挪

        static Constants() {
            RolePermissionMap.Add(RoleType.Parent
                , new List<string>{ "Setting_ChildClassInfo" });
            RolePermissionMap.Add(RoleType.Teacher
                , new List<string> { "Setting_ClassInfo" });
            RolePermissionMap.Add(RoleType.Master
                , new List<string> { "Setting_KGInfo" });
        }

        public static Dictionary<RoleType, List<String>> RolePermissionMap = new Dictionary<RoleType, List<string>>();
        public const int PAGESIZE = 10;

        #endregion

        #region OSS

        public const string PNGSuffix = ".png";
        public const string ImageServerPath = "http://babybus.emolbase.com/";
        public const string ThumbServerPath = "http://image.emolbase.com/";
        public const string ThumbRule = "@1e_80w_80h_1c_0i_1o_1x.png";
        public const string ThumbRule40 = "@1e_40w_40h_1c_0i_1o_1x.png";
        public const string OSSKey = "oMhFxiUEplUV9xIt";
        public const string OSSSecret = "OZfbMNMOP8iHNJOvvZld1ZNFGcdijj";
        public const string BucketName = "babybus-image";
        public const string OSSEndPoint = "http://oss-cn-qingdao.aliyuncs.com";

        #endregion

        #region Constants value

        public const int MiniPasswordLength = 6;
        public const int RefreshTime = 1000;
        public const int ProgressLongTime = 10000;
        public const int MaxRetrySendImageTime = 3;
        public const int MaxContentLength = 1000;

        #endregion

        
    }

    public class UIConstants {
        public const float CornerRadius = 4;
        public const string RETURN_PREVIOUS_PAGE = "返回";

    }
}
