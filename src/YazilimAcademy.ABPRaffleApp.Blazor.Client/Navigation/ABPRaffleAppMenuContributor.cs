using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using YazilimAcademy.ABPRaffleApp.Localization;
using YazilimAcademy.ABPRaffleApp.Permissions;
using YazilimAcademy.ABPRaffleApp.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.LanguageManagement.Blazor.Menus;

namespace YazilimAcademy.ABPRaffleApp.Blazor.Client.Navigation;

public class ABPRaffleAppMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public ABPRaffleAppMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ABPRaffleAppResource>();

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        context.Menu.AddItem(new ApplicationMenuItem(
            ABPRaffleAppMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));

        //HostDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                ABPRaffleAppMenus.HostDashboard,
                l["Menu:Dashboard"],
                "/HostDashboard",
                icon: "fa fa-chart-line",
                order: 2
            ).RequirePermissions(ABPRaffleAppPermissions.Dashboard.Host)
        );

        //Administration->Identity
        administration.SetSubItemOrder(IdentityProMenus.GroupName, 2);

        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenus.GroupName, 4);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMenus.GroupName, 6);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 7);

        var bookStoreMenu = new ApplicationMenuItem(
            "BooksStore",
            l["Menu:Books"],
            icon: "fa fa-book"
        );

        context.Menu.AddItem(bookStoreMenu);

        var rafflesMenu = new ApplicationMenuItem(
            "Raffles",
            l["Menu:Raffles"],
            icon: "fa fa-gift"
        );

        context.Menu.AddItem(rafflesMenu);

        //CHECK the PERMISSION
        if (await context.IsGrantedAsync(ABPRaffleAppPermissions.Books.Default))
        {
            bookStoreMenu.AddItem(new ApplicationMenuItem(
                "BooksStore.Books",
                l["Menu:Books"],
                url: "/books"
            ));
        }

        if (await context.IsGrantedAsync(ABPRaffleAppPermissions.Raffles.Default))
        {
            rafflesMenu.AddItem(new ApplicationMenuItem(
                "Raffles.Raffles",
                l["Menu:Raffles"],
                url: "/raffles"
            ));
        }
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-cog",
            order: 1000,
            target: "_blank").RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.SecurityLogs",
            accountStringLocalizer["MySecurityLogs"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/SecurityLogs",
            icon: "fa fa-user-shield",
            order: 1001,
            target: "_blank").RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Sessions",
            accountStringLocalizer["Sessions"],
            url: $"{authServerUrl.EnsureEndsWith('/')}Account/Sessions",
            icon: "fa fa-clock",
            order: 1002,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }
}
