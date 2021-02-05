using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Web;

namespace WesleyCore.Grpc
{
    /// <summary>
    /// Grpc异常处理
    /// </summary>
    public class LoggerGrpcInterceptor : Interceptor
    {
        private readonly ILogger<LoggerGrpcInterceptor> _logger;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger"></param>
        public LoggerGrpcInterceptor(ILogger<LoggerGrpcInterceptor> logger)
        {
            _logger = logger;
        }

        #region 服务端异常

        /// <summary>
        /// 服务端
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall(context);
            try
            {
                return await continuation(request, context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured when calling {context.Method}");
                if (e is WlException)
                {
                    throw new RpcException(new Status(StatusCode.Unknown, e.Message));
                }
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }

        private void LogCall(ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            _logger.LogDebug($"Starting call. Request: {httpContext.Request.Path}");
        }

        #endregion 服务端异常

        #region 客户端异常

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
           TRequest request,
           ClientInterceptorContext<TRequest, TResponse> context,
           AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            LogCall(context.Method);

            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> t)
        {
            try
            {
                var response = await t;
                _logger.LogDebug($"Response received: {response}");
                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Call error: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="method"></param>
        private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method) where TRequest : class where TResponse : class
        {
            _logger.LogDebug($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        }

        #endregion 客户端异常
    }
}