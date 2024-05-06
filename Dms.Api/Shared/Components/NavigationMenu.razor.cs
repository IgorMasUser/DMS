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
}