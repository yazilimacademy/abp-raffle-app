using Xunit;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore;

[CollectionDefinition(ABPRaffleAppTestConsts.CollectionDefinitionName)]
public class ABPRaffleAppEntityFrameworkCoreCollection : ICollectionFixture<ABPRaffleAppEntityFrameworkCoreFixture>
{

}
