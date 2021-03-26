﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WesleyCore;
using WesleyCore.Web;
using WesleyUntity;

namespace Wesley.Filter
{
    /// <summary>
    ///
    /// </summary>
    public class ExceptionResultFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="Logger"></param>
        public ExceptionResultFilter(ILogger<ExceptionResultFilter> Logger)
        {
            _logger = Logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var re = new BizResult() { Success = false };
            if (context.Exception is WlException)
            {
                var ex = context.Exception;
                re.Message = ex.Message;
            }
            else if (context.Exception.InnerException is WlException)
            {
                var ex = context.Exception.InnerException;
                re.Message = ex.Message;
            }
            else
            {
                var ex = context.Exception;
#if DEBUG
                _logger.LogError(ex.Message, ex);
                re.Message = string.Format("{0}{1}", ex.Message, ex.ToString());
#else
                _logger.LogError(ex.Message, ex);
                re.Message = "系统开了一点小差";
#endif
            }
            context.Result = new JsonResult(re);
        }
    }
}