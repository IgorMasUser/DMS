using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Dms.Core.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace Dms.Api.Pages
{
    public partial class Logs
    {

        private string Title { get; set; } = "Logs";
        private MudDataGrid<LogDto> _table = default!;
        private readonly LogDto _currentDto = new();
        private int _defaultPageSize = 15;
        private List<LogDto> logDtos = null;
        private bool _selectedDefaultSortOrder = false;
        private int totalRecords;
        private MudDateRangePicker _picker;
        private DateRange dateRange = new DateRange(DateTime.UtcNow.AddDays(-5).Date, DateTime.UtcNow.Date);

        [Parameter]
        public int CurrentPage { get; set; }

        [Inject]
        private ILogsDbContext dbContext { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        ILogger<Logs> Logger { get; set; }

        private bool _loading;

        private LogLevel? filterLogLevel;
        private LogListView filterLogListView = LogListView.All;
        private string? filterKeyword;

        protected override void OnInitialized()
        {
            Title ="Logs";
        }

        private async Task<GridData<LogDto>> ServerReload(GridState<LogDto> state)
        {
            try
            {
                _loading = true;

                var sort = state.SortDefinitions.FirstOrDefault();
                var sortDescending = sort is not null ? sort.Descending : false;
                LoadData(state);


                var logs = await GetLogsData();
                logDtos = Mapper.Map<List<LogDto>>(logs);

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
                _loading = false;
            }
        }

        private async Task OnSelectedRangeClickAsync()
        {
            _picker.Close();
            await _table.ReloadServerData();
        }


        private void LoadData(GridState<LogDto> state)
        {
           
        }

        //private async Task OnChangedListView(LogListView listview)
        //{
        //    filterLogListView = listview;
        //    queryParams.logListView = listview;
        //    await _table.ReloadServerData();
        //}

        //private async Task OnChangedLevel(LogLevel? level)
        //{
        //    filterLogLevel = level;
        //    if (level.HasValue)
        //    {
        //        queryParams.Level = level.Value;
        //    }
        //    else
        //    {
        //        queryParams.Level = LogLevel.Trace;
        //    }
        //    await _table.ReloadServerData();
        //}

        //private async Task OnSearch(string text)
        //{
        //    filterKeyword = text;
        //    queryParams.SearchedText = text;
        //    await _table.ReloadServerData();
        //}

        //private async Task OnRefresh()
        //{
        //    _defaultPageSize = 15;
        //    queryParams.Level = default;
        //    queryParams.SearchedText = default;
        //    queryParams.logListView = default;
        //    _selectedDefaultSortOrder = true;
        //    filterKeyword = default;
        //    filterLogListView = LogListView.All;
        //    skipLogs = default;
        //    takeLogs = _defaultPageSize;
        //    _table.SortDefinitions.Clear();
        //    filterLogLevel = default;
        //    await _table.SetRowsPerPageAsync(_defaultPageSize);
        //    _table.CurrentPage = 0;
        //    await _table.ReloadServerData();
        //}

        private async Task<List<Logger>> GetLogsData()
        {
            try
            {
                var logs = dbContext.Loggers.ToList();
                return logs;

            }
            catch (Exception ex)
            {
                Logger.LogError($"Error occurred while getting paginated logs: {ex.Message}");
            }

            return new List<Logger>();
        }

    }
}
