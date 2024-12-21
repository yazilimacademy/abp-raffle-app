using YazilimAcademy.ABPRaffleApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace YazilimAcademy.ABPRaffleApp.Permissions;

public class ABPRaffleAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ABPRaffleAppPermissions.GroupName);

        myGroup.AddPermission(ABPRaffleAppPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(ABPRaffleAppPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        var booksPermission = myGroup.AddPermission(ABPRaffleAppPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ABPRaffleAppPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ABPRaffleAppPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ABPRaffleAppPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ABPRaffleAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ABPRaffleAppResource>(name);
    }
}
