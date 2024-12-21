using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ABPRaffleAppDbContextFactory : IDesignTimeDbContextFactory<ABPRaffleAppDbContext>
{
    public ABPRaffleAppDbContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        var configuration = BuildConfiguration();
        
        ABPRaffleAppEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ABPRaffleAppDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));
        
        return new ABPRaffleAppDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../YazilimAcademy.ABPRaffleApp.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
