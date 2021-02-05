using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Net;
using WesleyCore.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WesleyCore.User
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
            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddCommandLine(args).Build();
            BuildDB();
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        private static void BuildDB()
        {
            try
            {
                var dbFactory = new UserContextFactory();

                var db = dbFactory.CreateDbContext();
                Console.WriteLine("取得数据库实例");
                //更新数据库
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                    Console.WriteLine("迁移已完成");
                }
                else
                {
                    Console.WriteLine("没有需要迁移的项目");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("迁移失败");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
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
                    //webBuilder.ConfigureKestrel(options =>
                    //{
                    //    options.Listen(IPAddress.Any, 5003, listenOptions =>
                    //    {
                    //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    //    });
                    //});
                    webBuilder.UseStartup<Startup>();
                });
    }
}