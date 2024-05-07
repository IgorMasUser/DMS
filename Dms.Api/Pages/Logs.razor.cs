//using AutoMapper;
//using Microsoft.AspNetCore.Components;
//using MudBlazor;
//using System.Linq.Expressions;

//namespace Dms.Api.Pages
//{
//    public partial class Logs
//    {

//        private string Title { get; set; } = "Logs";
//        private MudDataGrid<LogDto> _table = default!;
//        private readonly LogDto _currentDto = new();
//        private int _defaultPageSize = 15;
//        private List<LogDto> logDtos = null;
//        private QueryParams queryParams = new QueryParams();
//        private bool _selectedDefaultSortOrder = false;
//        private int totalRecords;
//        private int takeLogs;
//        private int skipLogs;
//        private MudDateRangePicker _picker;
//        private DateRange dateRange = new DateRange(DateTime.UtcNow.AddDays(-5).Date, DateTime.UtcNow.Date);

//        [Parameter]
//        public int CurrentPage { get; set; }

//        [Inject]
//        private IDataStoreAcessRepositoryFactory? dataStoreAcessRepositoryFactory { get; set; }

//        [Inject]
//        public IMapper Mapper { get; set; }

//        [Inject]
//        ILogger<Logs> Logger { get; set; }

//        private bool _loading;

//        private LogLevel? filterLogLevel;
//        private LogListView filterLogListView = LogListView.All;
//        private string? filterKeyword;

//        protected override void OnInitialized()
//        {
//            Title = L["Logs"];
//        }

//        private async Task<GridData<LogDto>> ServerReload(GridState<LogDto> state)
//        {
//            try
//            {
//                _loading = true;

//                var sort = state.SortDefinitions.FirstOrDefault();
//                var sortDescending = sort is not null ? sort.Descending : false;
//                LoadData(state);

//                if (_selectedDefaultSortOrder)
//                {
//                    queryParams.sortByDescending = false;
//                    _selectedDefaultSortOrder = false;
//                }
//                else
//                {
//                    queryParams.sortByDescending = sortDescending;
//                }

//                var logs = await GetLogsData(queryParams, (skipLogs, takeLogs));
//                logDtos = Mapper.Map<List<LogDto>>(logs.Data);
//                totalRecords = logs.TotalItems;

//                if (logDtos != null)
//                {
//                    return new GridData<LogDto> { TotalItems = totalRecords, Items = logDtos };
//                }
//                else
//                {
//                    return new GridData<LogDto> { TotalItems = 0, Items = new List<LogDto>() };
//                }
//            }
//            finally
//            {
//                _loading = false;
//            }
//        }

//        private async Task OnSelectedRangeClickAsync()
//        {
//            _picker.Close();
//            await _table.ReloadServerData();
//        }


//        private void LoadData(GridState<LogDto> state)
//        {
//            _defaultPageSize = state.PageSize;

//            if (state.Page == 0)
//            {
//                CurrentPage = state.Page;
//                skipLogs = state.Page;
//                takeLogs = _defaultPageSize;
//            }
//            else
//            {
//                CurrentPage = state.Page;
//                skipLogs = CurrentPage * _defaultPageSize;
//                takeLogs = _defaultPageSize;
//            }
//        }

//        private async Task OnChangedListView(LogListView listview)
//        {
//            filterLogListView = listview;
//            queryParams.logListView = listview;
//            await _table.ReloadServerData();
//        }

//        private async Task OnChangedLevel(LogLevel? level)
//        {
//            filterLogLevel = level;
//            if (level.HasValue)
//            {
//                queryParams.Level = level.Value;
//            }
//            else
//            {
//                queryParams.Level = LogLevel.Trace;
//            }
//            await _table.ReloadServerData();
//        }

//        private async Task OnSearch(string text)
//        {
//            filterKeyword = text;
//            queryParams.SearchedText = text;
//            await _table.ReloadServerData();
//        }

//        private async Task OnRefresh()
//        {
//            _defaultPageSize = 15;
//            queryParams.Level = default;
//            queryParams.SearchedText = default;
//            queryParams.logListView = default;
//            _selectedDefaultSortOrder = true;
//            filterKeyword = default;
//            filterLogListView = LogListView.All;
//            skipLogs = default;
//            takeLogs = _defaultPageSize;
//            _table.SortDefinitions.Clear();
//            filterLogLevel = default;
//            await _table.SetRowsPerPageAsync(_defaultPageSize);
//            _table.CurrentPage = 0;
//            await _table.ReloadServerData();
//        }

//        private async Task<PaginatedResult<Logger>> GetLogsData(QueryParams queryParams, (int skip, int take) paging)
//        {
//            try
//            {
//                var logReader = dataStoreAcessRepositoryFactory?.GetDomainQueryableStoreRepository<ILoggerDataStore>();
//                if (logReader != null)
//                {
//                    Expression<Func<Logger, bool>> lambda = c => true;

//                    if (queryParams.Level != default(LogLevel))
//                    {
//                        lambda = lambda.And(l => l.Level.Equals(queryParams.Level.ToString()));
//                    }
//                    if (queryParams.SearchedText != null)
//                    {
//                        lambda = lambda.And(l =>
//                            (l.Message != null && l.Message.Contains(queryParams.SearchedText)) ||
//                            (l.Level != null && l.Level.Contains(queryParams.SearchedText)) ||
//                            (l.UserName != null && l.UserName.Contains(queryParams.SearchedText)) ||
//                            (l.CorrelationId != null && l.CorrelationId.Contains(queryParams.SearchedText)));
//                    }

//                    switch (queryParams.logListView)
//                    {
//                        case LogListView.LastWeek:
//                            var lastWeek = DateTime.UtcNow.AddDays(-7);
//                            lambda = lambda.And(l => lastWeek <= l.TimeStamp && l.TimeStamp <= DateTime.Now);
//                            break;
//                        case LogListView.LastDay:
//                            var today = DateTime.UtcNow.AddDays(-1);
//                            lambda = lambda.And(l => today <= l.TimeStamp && l.TimeStamp <= DateTime.Now);
//                            break;
//                        case LogListView.LastHour:
//                            var lastHour = DateTime.UtcNow.AddHours(-1);
//                            lambda = lambda.And(l => lastHour <= l.TimeStamp && l.TimeStamp <= DateTime.Now);
//                            break;
//                        case LogListView.LastFiveMinutes:
//                            var lastFiveMinutes = DateTime.UtcNow.AddMinutes(-5);
//                            lambda = lambda.And(l => lastFiveMinutes <= l.TimeStamp && l.TimeStamp <= DateTime.Now);
//                            break;
//                        case LogListView.CustomRange:
//                            if (dateRange.Start <= dateRange.End)
//                            {
//                                var start = dateRange.Start.Value.Date.AddHours(00).AddMinutes(00).AddSeconds(1).ToUniversalTime();
//                                var end = dateRange.End.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime();
//                                lambda = lambda.And(l => start <= l.TimeStamp && l.TimeStamp <= end);
//                                break;
//                            }
//                            else
//                            {
//                                break;
//                            }
//                    }

//                    var logs = await logReader.ReadAsync(
//                        lambda,
//                        paging,
//                        orderBy: l => l.TimeStamp,
//                        orderByDescending: queryParams.sortByDescending ? null : (Expression<Func<Logger, object>>)(l => l.TimeStamp)
//                    );
//                    return logs;
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError($"Error occurred while getting paginated logs: {ex.Message}");
//            }

//            return new PaginatedResult<Logger>(0, new List<Logger>());
//        }

//    }
//}
