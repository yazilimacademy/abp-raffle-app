using Volo.Abp.Modularity;

namespace YazilimAcademy.ABPRaffleApp;

/* Inherit from this class for your domain layer tests. */
public abstract class ABPRaffleAppDomainTestBase<TStartupModule> : ABPRaffleAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
