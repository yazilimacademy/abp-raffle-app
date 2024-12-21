using YazilimAcademy.ABPRaffleApp.Localization;
using Volo.Abp.Application.Services;

namespace YazilimAcademy.ABPRaffleApp;

/* Inherit your application services from this class.
 */
public abstract class ABPRaffleAppAppService : ApplicationService
{
    protected ABPRaffleAppAppService()
    {
        LocalizationResource = typeof(ABPRaffleAppResource);
    }
}
