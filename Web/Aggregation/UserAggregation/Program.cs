using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserAggregation
{
    /// <summary>
    ///
    /// </summary>
    public class Program
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //命令参数
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddCommandLine(args).Build();
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureLogging((hostingContext, loggingbuilder) =>
             {
                 loggingbuilder.AddFilter("System", LogLevel.Warning); //过滤掉系统默认的一些日志
                 loggingbuilder.AddFilter("Microsoft", LogLevel.Warning);//过滤掉系统默认的一些日志
                 loggingbuilder.ClearProviders();
                 loggingbuilder.AddConsole();
                 loggingbuilder.AddDebug();
                 //不带参数：表示log4net.config的配置文件就在应用程序根目录下，也可以指定配置文件的路径
                 loggingbuilder.AddLog4Net();
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}