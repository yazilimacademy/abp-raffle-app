using YazilimAcademy.ABPRaffleApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace YazilimAcademy.ABPRaffleApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ABPRaffleAppEntityFrameworkCoreModule),
    typeof(ABPRaffleAppApplicationContractsModule)
)]
public class ABPRaffleAppDbMigratorModule : AbpModule
{
}
