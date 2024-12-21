using Microsoft.Extensions.Localization;
using YazilimAcademy.ABPRaffleApp.Localization;
using Microsoft.Extensions.Localization;
using YazilimAcademy.ABPRaffleApp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace YazilimAcademy.ABPRaffleApp.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class ABPRaffleAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ABPRaffleAppResource> _localizer;

    public ABPRaffleAppBrandingProvider(IStringLocalizer<ABPRaffleAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
