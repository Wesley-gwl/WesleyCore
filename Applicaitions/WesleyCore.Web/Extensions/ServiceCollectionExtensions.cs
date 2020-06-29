using Microsoft.Extensions.DependencyInjection;

namespace WesleyCore.Web.Extensions
{
    /// <summary>
    /// 数据库注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlDomainContext(this.IServiceCollection services, string connectionString)
        {
            return services.AddDomainContext(builder =>
            {
                builder.UseMySql(connectionString);
            });
        }
    }
}