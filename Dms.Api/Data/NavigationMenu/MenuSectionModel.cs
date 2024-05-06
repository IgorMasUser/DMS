namespace Dms.Api.Services.NavigationMenu;

public class MenuSectionModel
{
    public string Title { get; set; } = string.Empty;
    public List<MenuSectionItemModel>? SectionItems { get; set; }
}