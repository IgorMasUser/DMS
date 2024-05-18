using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Common.UIModels;
using Dms.Core.Application.Common.UIModels.Enums;
using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Dms.Api.Pages
{
    public partial class Logs
    {
        private string Title { get; set; } = "Logs";
        private MudDataGrid<LogDto> table = default!;
        private readonly LogDto currentDto = new();
        private int defaultPageSize = 15;
        private List<LogDto> logDtos = null;
        private bool selectedDefaultSortOrder = false;
        private int totalRecords;
        private MudDateRangePicker picker;
        private LogsQueryParams queryParams = new();
        private DateRange dateRange = new DateRange(DateTime.UtcNow.AddDays(-5).Date, DateTime.UtcNow.Date);

        [Parameter]
        public int CurrentPage { get; set; }

        [Inject]
        private IDmsDbContext dbContext { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        ILogger<Logs> Logger { get; set; }

        private bool loading;

        private LogLevel? filterLogLevel;
        private LogListView filterLogListView = LogListView.All;
        private string? filterKeyword;

        private async Task<GridData<LogDto>> ServerReload(GridState<LogDto> state)
        {
            try
            {
                loading = true;

                var sort = state.SortDefinitions.FirstOrDefault();
                var sortDescending = sort is not null ? sort.Descending : false;
                SetUpDataLoading(state);

                if (selectedDefaultSortOrder)
                {
                    queryParams.sortByDescending = false;
                    selectedDefaultSortOrder = false;
                }
                else
                {
                    queryParams.sortByDescending = sortDescending;
                }

                var logs = await GetLogsData(queryParams);
                logDtos = Mapper.Map<List<LogDto>>(logs);
                totalRecords = logs.Count();

                if (logDtos != null)
                {
                    return new GridData<LogDto> { TotalItems = totalRecords, Items = logDtos };
                }
                else
                {
                    return new GridData<LogDto> { TotalItems = 0, Items = new List<LogDto>() };
                }
            }
            finally
            {
                loading = false;
            }
        }

        private async Task OnSelectedRangeClickAsync()
        {
            picker.Close();
            await table.ReloadServerData();
        }


        private void SetUpDataLoading(GridState<LogDto> state)
        {
            defaultPageSize = state.PageSize;

            if (state.Page == 0)
            {
                CurrentPage = state.Page;
                queryParams.skipLogs = state.Page;
                queryParams.takeLogs = defaultPageSize;
            }
            else
            {
                CurrentPage = state.Page;
                queryParams.skipLogs = CurrentPage * defaultPageSize;
                queryParams.takeLogs = defaultPageSize;
            }
        }

        private async Task OnChangedListView(LogListView listview)
        {
            filterLogListView = listview;
            queryParams.logListView = listview;
            await table.ReloadServerData();
        }

        private async Task OnChangedLevel(LogLevel? level)
        {
            filterLogLevel = level;
            if (level.HasValue)
            {
                queryParams.Level = level.Value;
            }
            else
            {
                queryParams.Level = LogLevel.Trace;
            }
            await table.ReloadServerData();
        }

        private async Task OnSearch(string text)
        {
            filterKeyword = text;
            queryParams.SearchedText = text;
            await table.ReloadServerData();
        }

        private async Task OnRefresh()
        {
            defaultPageSize = 15;
            queryParams.Level = default;
            queryParams.SearchedText = default;
            queryParams.logListView = default;
            selectedDefaultSortOrder = true;
            filterKeyword = default;
            filterLogListView = LogListView.All;
            queryParams.skipLogs = default;
            queryParams.takeLogs = defaultPageSize;
            table.SortDefinitions.Clear();
            filterLogLevel = default;
            await table.SetRowsPerPageAsync(defaultPageSize);
            table.CurrentPage = 0;
            await table.ReloadServerData();
        }

        private async Task<List<Logger>> GetLogsData(LogsQueryParams queryParams)
        {
            try
            {
                IQueryable<Logger> query = dbContext.Loggers;

                if (queryParams.Level != default(LogLevel))
                {
                    query = query.Where(l => l.Level == queryParams.Level.ToString());
                }

                if (!string.IsNullOrWhiteSpace(queryParams.SearchedText))
                {
                    query = query.Where(l =>
                        (l.Message != null && l.Message.Contains(queryParams.SearchedText)) ||
                        (l.Level != null && l.Level.Contains(queryParams.SearchedText)) ||
                        (l.CorrelationId != null && l.CorrelationId.Contains(queryParams.SearchedText)));
                }

                switch (queryParams.logListView)
                {
                    case LogListView.Last10days:
                        var last10days = DateTime.UtcNow.AddDays(-10);
                        query = query.Where(l => l.TimeStamp >= last10days);
                        break;
                    case LogListView.LastDay:
                        var today = DateTime.UtcNow.AddDays(-1);
                        query = query.Where(l => l.TimeStamp >= today);
                        break;
                    case LogListView.LastHour:
                        var lastHour = DateTime.UtcNow.AddHours(-1);
                        query = query.Where(l => l.TimeStamp >= lastHour);
                        break;
                    case LogListView.CustomRange:
                        if (dateRange?.Start <= dateRange?.End)
                        {
                            DateTime start = dateRange.Start.Value;
                            DateTime end = dateRange.End.Value;
                            query = query.Where(l => l.TimeStamp >= start && l.TimeStamp <= end);
                        }
                        break;
                }

                if (queryParams.sortByDescending)
                {
                    query = query.OrderByDescending(l => l.TimeStamp);
                }
                else
                {
                    query = query.OrderBy(l => l.TimeStamp);
                }

                var logs = await query
                    .Skip(queryParams.skipLogs)
                    .Take(queryParams.takeLogs)
                    .ToListAsync();

                return logs;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error occurred while getting logs: {ex.Message}");
                return new List<Logger>();
            }
        }
    }
}
