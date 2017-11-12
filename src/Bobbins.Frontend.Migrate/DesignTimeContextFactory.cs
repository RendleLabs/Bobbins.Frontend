using System.Linq;
using Bobbins.Frontend.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bobbins.Frontend.Migrate
{
    [UsedImplicitly]
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private const string LocalPostgres = "Host=localhost;Database=frontend;Username=bobbins;Password=secretsquirrel";

        private static readonly string MigrationAssemblyName = typeof(DesignTimeContextFactory).Assembly.GetName().Name;

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(args.FirstOrDefault() ?? LocalPostgres, b => b.MigrationsAssembly(MigrationAssemblyName));
            return new ApplicationDbContext(builder.Options);
        }
}
}
