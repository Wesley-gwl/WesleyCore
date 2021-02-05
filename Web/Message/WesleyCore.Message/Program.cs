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
            //�������
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
                var dbFactory = new MessageContextFactory();

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}