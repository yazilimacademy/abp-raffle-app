using Volo.Abp.Modularity;

namespace YazilimAcademy.ABPRaffleApp;

[DependsOn(
    typeof(ABPRaffleAppApplicationModule),
    typeof(ABPRaffleAppDomainTestModule)
)]
public class ABPRaffleAppApplicationTestModule : AbpModule
{

}
