using System.ComponentModel;

namespace Dms.Core.Domain.Enums
{
    public enum LogListView
    {
        [Description("All")] All,
        [Description("Created Toady")] CreatedToday,
        [Description("View of the last 10 days")] Last10days
    }
}
