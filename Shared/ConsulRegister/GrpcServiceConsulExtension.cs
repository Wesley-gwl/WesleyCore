using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WesleyRedis;

namespace ConsulRegister
{
    /// <summary>
    /// gRPC服务发现
    /// </summary>
    public static class GrpcServiceConsulExtension
    {
        public static string ConsulAddress = "http://localhost:8500";

        /// <summary>
        /// gRPC服务发现 没有token https
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static async Task<string> GetGrpcServiceHttps(string serviceName)
        {
            var address = string.Empty;
            //优先从redis里获取
            var list = RedisClient.RedisCt.GetStringKey<List<string>>(serviceName);
            if (list != null && list.Count > 0)
            {
                address = list[new Random().Next(0, list.Count - 1)];
            }
            else//备用方案,主方案通过线程去自动更新redis内服务
            {
                var consulClient = new ConsulClient(c => { c.Address = new Uri(ConsulAddress); });
                var services = await consulClient.Health.Service(serviceName, string.Empty, true);
                if (services.Response.Length == 0)
                {
                    throw new Exception($"未发现服务 {serviceName}");
                }
                var service = services.Response[new Random().Next(0, services.Response.Length - 1)];
                address = $"https://{service.Service.Address}:{service.Service.Port}";
                list = new List<string>();
                foreach (var item in services.Response)
                {
                    list.Add($"https://{item.Service.Address}:{item.Service.Port}");
                }
                RedisClient.RedisCt.SetStringKey(serviceName, list, TimeSpan.FromMinutes(5));
            }
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
            var address = string.Empty;
            //优先从redis里获取
            var list = RedisClient.RedisCt.GetStringKey<List<string>>(serviceName);
            if (list != null && list.Count > 0)
            {
                address = list[new Random().Next(0, list.Count - 1)];
            }
            else
            {
                var consulClient = new ConsulClient(c => { c.Address = new Uri(ConsulAddress); });
                var services = await consulClient.Health.Service(serviceName, string.Empty, true);
                if (services.Response.Length == 0)
                {
                    throw new Exception($"未发现服务 {serviceName}");
                }
                var service = services.Response[new Random().Next(0, services.Response.Length - 1)];
                address = $"http://{service.Service.Address}:{service.Service.Port}";
                list = new List<string>();
                foreach (var item in services.Response)
                {
                    list.Add($"http://{item.Service.Address}:{item.Service.Port}");
                }
                RedisClient.RedisCt.SetStringKey(serviceName, list, TimeSpan.FromMinutes(5));
            }
            Console.WriteLine($"获取服务地址成功：{address}");
            return address;
        }
    }
}