using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YazilimAcademy.ABPRaffleApp.Data;
using Volo.Abp.DependencyInjection;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore;

public class EntityFrameworkCoreABPRaffleAppDbSchemaMigrator
    : IABPRaffleAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreABPRaffleAppDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ABPRaffleAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ABPRaffleAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
