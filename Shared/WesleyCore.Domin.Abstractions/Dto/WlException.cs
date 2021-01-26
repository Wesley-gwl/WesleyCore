using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyCore
{
    /// <summary>
    /// 异常
    /// </summary>
    public class WlException : Exception
    {
        /// <summary>
        /// 业务异常构造函数
        /// </summary>
        /// <param name="message"></param>
        public WlException(string message = null, int errorCode = 9999, object errorData = null)
        {
            _bizMessage = message;
            ErrorCode = errorCode;
            ErrorData = errorData;
        }

        private string _bizMessage;

        /// <summary>
        ///
        /// </summary>
        public override string Message
        {
            get { return _bizMessage; }
        }

        /// <summary>
        /// 错误编码
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// 错误数据
        /// </summary>
        public object ErrorData { get; private set; }
    }
}