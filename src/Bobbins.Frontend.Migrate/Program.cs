using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RendleLabs.EntityFrameworkCore.MigrateHelper;

namespace Bobbins.Frontend.Migrate
{
    [UsedImplicitly]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole(LogLevel.Information);
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Trying migration...");
            await new MigrationHelper(loggerFactory).TryMigrate(args);
            logger.LogInformation("Done.");
        }
    }
}
