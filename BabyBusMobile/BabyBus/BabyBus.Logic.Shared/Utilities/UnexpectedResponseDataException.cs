using System;

namespace BabyBus.Logic.Shared
{
    public class UnexpectedResponseDataException:Exception
    {
        public const string RETURN_ERROR_DATA = "返回数据非法";

        public UnexpectedResponseDataException()
            : base(RETURN_ERROR_DATA)
        {
        }
    }
}

