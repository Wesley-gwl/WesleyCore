using Microsoft.EntityFrameworkCore;
using WesleyCore.EntityFrameworkCore;

namespace WesleyPool.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<DomainContext> dbContextOptions,
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for WesleyPoolDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }

        public static void Configure(
          DbContextOptionsBuilder<MemberContext> dbContextOptions,
          string connectionString
          )
        {
            /* This is the single point to configure DbContextOptions for WesleyPoolDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}