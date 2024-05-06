using Dms.Api.Services.NavigationMenu;
using MudBlazor;


namespace Dms.Api.Services.Navigation;

public class MenuService : IMenuService
{
    public IEnumerable<MenuSectionModel> Features => _features;
    private readonly List<MenuSectionModel> _features = new()
    {
        new MenuSectionModel
        {
            SectionItems = new List<MenuSectionItemModel>
            {
                new() { Title = "Home", Icon = Icons.Material.Filled.Home, Href = "/" },
                new()
                {
                    Title = "DMS",
                    Icon = Icons.Material.Filled.Analytics,
                    Href = "/dms",
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Document service",
                            Href = "/pages/documents",
                            PageStatus = PageStatus.Completed
                        }
                    }
                }
            }
        },
        new MenuSectionModel
        {
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "System",
                    Icon = Icons.Material.Filled.Devices,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Logs",
                            Href = "/system/logs",
                            PageStatus = PageStatus.Completed
                        }
                    }
                }
            }
        }
    };
}