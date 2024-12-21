using YazilimAcademy.ABPRaffleApp.Samples;
using Xunit;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Applications;

[Collection(ABPRaffleAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ABPRaffleAppEntityFrameworkCoreTestModule>
{

}
