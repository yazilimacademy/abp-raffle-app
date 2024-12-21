using Volo.Abp.Modularity;

namespace YazilimAcademy.ABPRaffleApp;

[DependsOn(
    typeof(ABPRaffleAppDomainModule),
    typeof(ABPRaffleAppTestBaseModule)
)]
public class ABPRaffleAppDomainTestModule : AbpModule
{

}
