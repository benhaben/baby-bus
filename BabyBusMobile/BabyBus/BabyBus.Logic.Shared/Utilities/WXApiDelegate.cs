using System;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
    public class BaseReqOfBabyBus
    {

        public int Type { get; set; }

        public  string OpenID { get; set; }
    }

    // @interface BaseResp : NSObject
    public class BaseRespOfBabyBus
    {
        public int ErrCode { get; set; }

        public string ErrStr { get; set; }

        public  int Type { get; set; }
    }

    public class  SendAuthRespOfBabyBus:BaseRespOfBabyBus
    {

        public string Code { get; set; }

        public string State { get; set; }

        public string Lang { get; set; }

        public string Country { get; set; }
    }

    public interface IWxApiDelegateAdapter
    {
        void OnReq(BaseReqOfBabyBus req);

        void  OnResp(BaseRespOfBabyBus resp);
    }

   
}

