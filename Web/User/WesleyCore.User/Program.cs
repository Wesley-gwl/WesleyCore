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
        /// �������ݿ�
        /// </summary>
        private static void BuildDB()
        {
            try
            {
                var dbFactory = new UserContextFactory();

                var db = dbFactory.CreateDbContext();
                Console.WriteLine("ȡ�����ݿ�ʵ��");
                //�������ݿ�
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                    Console.WriteLine("Ǩ�������");
                }
                else
                {
                    Console.WriteLine("û����ҪǨ�Ƶ���Ŀ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ǩ��ʧ��");
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
                loggingbuilder.AddFilter("System", LogLevel.Warning); //���˵�ϵͳĬ�ϵ�һЩ��־
                loggingbuilder.AddFilter("Microsoft", LogLevel.Warning);//���˵�ϵͳĬ�ϵ�һЩ��־
                loggingbuilder.ClearProviders();
                loggingbuilder.AddConsole();
                loggingbuilder.AddDebug();
                //������������ʾlog4net.config�������ļ�����Ӧ�ó����Ŀ¼�£�Ҳ����ָ�������ļ���·��
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