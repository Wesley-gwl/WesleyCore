using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace ConsulRegister
{
    public static class RegisterToConsulExtension
    {
        /// <summary>
        /// Add Consul
        /// 添加consul
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            // configuration Consul register address
            //配置consul注册地址
            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceDiscovery"));

            //configuration Consul client
            //配置consul客户端
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(consulOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(consulOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        /// <summary>
        /// use Consul
        /// 使用consul
        /// The default health check interface format is http://host:port/HealthCheck
        /// 默认的健康检查接口格式是 http://host:port/Api/HealthCheck/Check
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IApplicationLifetime appLife = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            IOptions<ServiceDiscoveryOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ServiceDiscoveryOptions>>();
            //var features = app.Properties["server.Features"] as FeatureCollection;
            //var port = new Uri(features.Get<IServerAddressesFeature>()
            //    .Addresses
            //    .FirstOrDefault()).Port;
            var port = int.Parse(configuration["port"]);
            var ip = configuration["ip"];
            var http = configuration["http"];
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"application port is :{port}");

            #region 环境

            //var addressIpv4Hosts = NetworkInterface.GetAllNetworkInterfaces()
            //.OrderByDescending(c => c.Speed)
            //.Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);

            //foreach (var item in addressIpv4Hosts)
            //{
            //    var props = item.GetIPProperties();
            //    //this is ip for ipv4
            //    //这是ipv4的ip地址
            //    var firstIpV4Address = props.UnicastAddresses
            //        .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
            //        .Select(c => c.Address)
            //        .FirstOrDefault().ToString();
            //    var serviceId = $"{serviceOptions.Value.ServiceName}_{firstIpV4Address}:{port}";

            //    var httpCheck = new AgentServiceCheck()//服务健康检查
            //    {
            //        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),//每隔1分钟检查
            //        Interval = TimeSpan.FromSeconds(30),//检测等待时间
            //        //this is default health check interface
            //        //这个是默认健康检查接口
            //        HTTP = $"{Uri.UriSchemeHttp}://{firstIpV4Address}:{port}/Api/HealthCheck/Check",
            //    };

            //    var registration = new AgentServiceRegistration()
            //    {
            //        Checks = new[] { httpCheck },
            //        Address = firstIpV4Address.ToString(),
            //        ID = serviceId,
            //        Name = serviceOptions.Value.ServiceName,
            //        Port = port
            //    };

            //    consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            //    //send consul request after service stop
            //    //当服务停止后想consul发送的请求
            //    appLife.ApplicationStopping.Register(() =>
            //    {
            //        consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            //    });

            //    Console.ForegroundColor = ConsoleColor.Blue;
            //    Console.WriteLine($"health check service:{httpCheck.HTTP}");
            //}

            #endregion 环境

            //register localhost address
            //注册本地地址
            var localhostregistration = new AgentServiceRegistration()
            {
                Checks = new[] { new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(new Random ().Next(10,30)),//间隔60s一次 检查
                    //Timeout = TimeSpan.FromSeconds(2),//检测等待时间
                    HTTP = $"{http??Uri.UriSchemeHttp}://{ip}:{port}/Api/HealthCheck/Check",
                } },
                Address = ip,
                ID = $"{ip}:{port}",
                Name = configuration["ServiceDiscovery:ServiceName"],//队伍名称
                Port = port
            };

            consul.Agent.ServiceRegister(localhostregistration).GetAwaiter().GetResult();

            //send consul request after service stop
            //当服务停止后想consul发送的请求
            appLife.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(localhostregistration.ID).GetAwaiter().GetResult();
            });

            return app;
        }
    }
}