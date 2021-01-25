using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsulRegister
{
    /// <summary>
    /// gRPC服务发现
    /// </summary>
    public static class GrpcServiceConsulExtension
    {
        /// <summary>
        /// gRPC服务发现 没有token https
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static async Task<string> GetGrpcServiceHttps(string serviceName)
        {
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://localhost:8500"));
            var services = await consulClient.Catalog.Service(serviceName);
            if (services.Response.Length == 0)
            {
                throw new Exception($"未发现服务 {serviceName}");
            }
            var service = services.Response[new Random().Next(0, services.Response.Length - 1)];
            var address = $"https://{service.ServiceAddress}:{service.ServicePort}";
            Console.WriteLine($"获取服务地址成功：{address}");
            return address;
        }

        /// <summary>
        /// gRPC服务发现 没有token http
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static async Task<string> GetGrpcServiceHttp(string serviceName)
        {
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://localhost:8500"));
            var services = await consulClient.Catalog.Service(serviceName);
            if (services.Response.Length == 0)
            {
                throw new Exception($"未发现服务 {serviceName}");
            }
            var service = services.Response[new Random().Next(0, services.Response.Length - 1)];
            var address = $"http://{service.ServiceAddress}:{service.ServicePort}";
            Console.WriteLine($"获取服务地址成功：{address}");
            return address;
        }
    }
}