using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Visor.Data.MySql.Identity
{
    public class DesignTimeIdDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        private static string _connectionString = "";
        public DesignTimeIdDbContextFactory()
        {

        }
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;

        }
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseMySQL(_connectionString);
            return new IdentityContext(builder.Options);
        }
    }
}
