using System.Linq;
using Bobbins.Frontend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bobbins.Frontend.Migrate
{
    public class DesignTimeFooContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public const string LocalPostgres = "Host=localhost;Database=foo;Username=vids;Password=secretsquirrel";
        public static readonly string MigrationAssemblyName =
            typeof(DesignTimeFooContextFactory).Assembly.GetName().Name;


        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(args.FirstOrDefault() ?? LocalPostgres, b => b.MigrationsAssembly(MigrationAssemblyName));
            return new ApplicationDbContext(builder.Options);
        }
}
}
