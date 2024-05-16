using System.ComponentModel;

namespace Dms.Core.Application.Common.UIModels.Enums
{
    public enum DocumentHistoryEvent
    {
        [Description("Document created")]
        Created = 0,
        [Description("Error creation")]
        FailedWhenCreated = 1
    }
}
