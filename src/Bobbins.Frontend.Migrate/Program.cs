using System;
using System.Threading.Tasks;
using RendleLabs.EntityFrameworkCore.MigrateHelper;

namespace Bobbins.Frontend.Migrate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new MigrationHelper().TryMigrate(args);
        }
    }
}
