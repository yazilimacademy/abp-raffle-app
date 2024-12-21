using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace YazilimAcademy.ABPRaffleApp.Data;

/* This is used if database provider does't define
 * IABPRaffleAppDbSchemaMigrator implementation.
 */
public class NullABPRaffleAppDbSchemaMigrator : IABPRaffleAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
