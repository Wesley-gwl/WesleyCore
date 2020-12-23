using WesleyUntity;

namespace WesleyCore.Web
{
    /// <summary>
    /// 返回数据实体
    /// </summary>
    public class BizResult
    {
        /// <summary>
        ///
        /// </summary>
        public BizResult()
        {
        }

        /// <summary>
        /// 结果为成功,设置成功的消息
        /// </summary>
        /// <param name="msg"></param>
        public BizResult(string msg)
        {
            Message = msg;
            Success = true;
        }

        /// <summary>
        /// 设置结果和提示消息
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="msg"></param>
        public BizResult(bool isSuccess, string msg)
        {
            Message = msg;
            Success = isSuccess;
        }

        /// <summary>
        /// 接口调用是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 返回的错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 不成功时的数据(如果需要)
        /// </summary>
        public object ErrorData { get; set; }

        /// <summary>
        /// 成功时的数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 输出格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BizResult<T> : BizResult
    {
        /// <summary>
        /// 构造
        /// </summary>
        public BizResult()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_data"></param>
        public BizResult(T _data)
        {
            Data = _data;
            Success = true;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="_data"></param>
        /// <param name="msg"></param>
        public BizResult(bool isSuccess, T _data, string msg)
        {
            Data = _data;
            Success = isSuccess;
            Message = msg;
        }

        /// <summary>
        /// 返回的数据（泛型）
        /// </summary>
        public new T Data { get; set; }
    }
}