using Dms.Api.Services.Navigation;
using Dms.Api.Services.NavigationMenu;
using Microsoft.AspNetCore.Components;

namespace Dms.Api.Shared.Components;

public partial class NavigationMenu : ComponentBase
{
    [Inject] private IMenuService MenuService { get; set; } = null!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    private IEnumerable<MenuSectionModel> MenuSections => MenuService.Features;
    private string AppName { get; set; } = "DMS";

    private bool Expanded(MenuSectionItemModel menu)
    {
        string href = NavigationManager.Uri[(NavigationManager.BaseUri.Length - 1)..];
        return menu is { IsParent: true, MenuItems: not null } &&
               menu.MenuItems.Any(x => !string.IsNullOrEmpty(x.Href) && x.Href.Equals(href));
    }
}