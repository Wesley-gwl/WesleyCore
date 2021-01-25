using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wesley.GrpcService;
using WesleyCore.User.Proto;

namespace UserAggregation.Controllers
{
    /// <summary>
    /// 用户聚合
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("permission")]//token验证
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 服务站点发现
        /// </summary>
        private readonly IGrpcServiceHelper grpcServiceHelper;

        /// <summary>
        /// 构造
        /// </summary>
        public UserController(IGrpcServiceHelper grpcServiceHelper)
        {
            this.grpcServiceHelper = grpcServiceHelper;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="paged"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserPagedOutput> GetUserPaged(int paged = 1, int rows = 20)
        {
            var  _header = new Grpc.Core.Metadata() { { "Authorization", $"{HttpContext.Request.Headers["Authorization"]}" } };
            var userServiceClient = await grpcServiceHelper.GetUserService();
            return await userServiceClient.GetUserListAsync(new GetUserPagedForm()
            {
                Paged = paged,
                Rows = rows
            }, _header);
        }
    }
}