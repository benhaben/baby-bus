namespace BabyBus.Core.Helper
{
    /// <summary>
    /// Controller相应View的封装类
    /// </summary>
    public class JsonResponser
    {
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
