using YazilimAcademy.ABPRaffleApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace YazilimAcademy.ABPRaffleApp.Blazor.Client;

public abstract class ABPRaffleAppComponentBase : AbpComponentBase
{
    protected ABPRaffleAppComponentBase()
    {
        LocalizationResource = typeof(ABPRaffleAppResource);
    }
}
