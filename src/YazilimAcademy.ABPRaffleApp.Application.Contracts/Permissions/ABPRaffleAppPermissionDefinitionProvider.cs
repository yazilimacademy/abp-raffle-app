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

        var rafflesPermission = myGroup.AddPermission(ABPRaffleAppPermissions.Raffles.Default, L("Permission:Raffles"));
        rafflesPermission.AddChild(ABPRaffleAppPermissions.Raffles.Create, L("Permission:Raffles.Create"));
        rafflesPermission.AddChild(ABPRaffleAppPermissions.Raffles.Edit, L("Permission:Raffles.Edit"));
        rafflesPermission.AddChild(ABPRaffleAppPermissions.Raffles.Delete, L("Permission:Raffles.Delete"));

        var participantsPermission = myGroup.AddPermission(ABPRaffleAppPermissions.Participants.Default, L("Permission:Participants"));
        participantsPermission.AddChild(ABPRaffleAppPermissions.Participants.Create, L("Permission:Participants.Create"));
        participantsPermission.AddChild(ABPRaffleAppPermissions.Participants.Edit, L("Permission:Participants.Edit"));
        participantsPermission.AddChild(ABPRaffleAppPermissions.Participants.Delete, L("Permission:Participants.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ABPRaffleAppResource>(name);
    }
}
