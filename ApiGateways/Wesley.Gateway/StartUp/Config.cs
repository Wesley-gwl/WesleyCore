using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyPC.Gateway
{
    /// <summary>
    /// 配置
    /// </summary>
    public static class Config
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
            };
        }

        /// <summary>
        ///  scopes define the API resources in your system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //可访问的API资源(资源名，资源描述)
            return new List<ApiResource>
            {
                new ApiResource("ApiA", "ApiA"),
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client_a", //访问客户端Id,必须唯一
                    //使用客户端授权模式，客户端只需要clientid和secrets就可以访问对应的api资源。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "piA",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile }
                },
                new  Client
                {
                    ClientId = "client_b",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "Api_B",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }
    }
}