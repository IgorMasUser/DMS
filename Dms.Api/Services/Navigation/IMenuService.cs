using Dms.Api.Services.NavigationMenu;

namespace Dms.Api.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
