using System.ComponentModel;

namespace Dms.Core.Application.Common.UIModels.Enums
{
    public enum LogListView
    {
        [Description("All")] All,
        [Description("View of the last hour")] LastHour,
        [Description("View of the last day")] LastDay,
        [Description("View of the last 10 days")] Last10days,
        [Description("Select custom range")] CustomRange
    }
}
