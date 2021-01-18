using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Grpc.Proto;

namespace GrpcService
{
    public class GreeterService : ILoginService.ILoginServiceBase
    {
        /// <summary>
        /// µÇÂ¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LoginResult> Login(LoginForm input)
        {
            return new LoginResult() { Token = "123" };
        }
    }
}