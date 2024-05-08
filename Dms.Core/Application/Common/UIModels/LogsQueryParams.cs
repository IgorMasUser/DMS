using Dms.Core.Application.Common.UIModels.Enums;
using Microsoft.Extensions.Logging;

namespace Dms.Core.Application.Common.UIModels
{
    public class LogsQueryParams
    {
        public LogLevel Level { get; set; }
        public string? SearchedText { get; set; }
        public LogListView logListView { get; set; }
        public bool sortByDescending { get; set; } = false;
        public int takeLogs { get; set; }
        public int skipLogs { get; set; }
    }
}
