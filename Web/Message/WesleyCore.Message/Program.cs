using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.EntityFrameworkCore;

namespace WesleyCore.Message
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
                var dbFactory = new MessageContextFactory();

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}