using YazilimAcademy.ABPRaffleApp.Books;
using Xunit;

namespace YazilimAcademy.ABPRaffleApp.EntityFrameworkCore.Applications.Books;

[Collection(ABPRaffleAppTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<ABPRaffleAppEntityFrameworkCoreTestModule>
{

}