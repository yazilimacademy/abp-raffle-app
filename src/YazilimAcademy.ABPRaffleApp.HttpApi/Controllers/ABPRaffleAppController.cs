using YazilimAcademy.ABPRaffleApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace YazilimAcademy.ABPRaffleApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ABPRaffleAppController : AbpControllerBase
{
    protected ABPRaffleAppController()
    {
        LocalizationResource = typeof(ABPRaffleAppResource);
    }
}
