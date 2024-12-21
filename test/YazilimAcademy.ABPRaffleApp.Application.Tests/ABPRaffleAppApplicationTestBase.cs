using Volo.Abp.Modularity;

namespace YazilimAcademy.ABPRaffleApp;

public abstract class ABPRaffleAppApplicationTestBase<TStartupModule> : ABPRaffleAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
