using YazilimAcademy.ABPRaffleApp.Samples;
using Xunit;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Domains;

[Collection(ABPRaffleAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ABPRaffleAppEntityFrameworkCoreTestModule>
{

}
