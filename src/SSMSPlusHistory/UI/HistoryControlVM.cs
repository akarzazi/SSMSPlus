namespace SSMSPlusHistory.UI.VM
{
    using Microsoft.SqlServer.Management.UI.VSIntegration;
    using SSMSPlusCore.App;
    using SSMSPlusCore.Integration;
    using SSMSPlusCore.Ui;
    using SSMSPlusHistory.Entities.Search;
    using SSMSPlusHistory.Repositories;
    using SSMSPlusHistory.Services.Filtering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class HistoryControlVM : ViewModelBase
    {
        private QueryItemRepository _itemsRepository;
        private IVersionProvider _versionProvider;
        private IServiceCacheIntegration _serviceCacheIntegration;

        public IAsyncCommand RequestItemsCommand { get; private set; }
        public IAsyncCommand ViewLoadedCommand { get; private set; }
        public Command<SearchFilterResultVM> OpenScriptCmd { get; }

        private bool _loadedOnce = false;

        public HistoryControlVM(QueryItemRepository itemsRepository, IVersionProvider versionProvider, IServiceCacheIntegration serviceCacheIntegration)
        {
            _itemsRepository = itemsRepository;
            _versionProvider = versionProvider;
            _serviceCacheIntegration = serviceCacheIntegration;
            RequestItemsCommand = new AsyncCommand(ExecuteRequestItemsAsync, CanExecuteSubmit, this.HandleError);
            ViewLoadedCommand = new AsyncCommand(OnViewLoadedAsync, CanExecuteSubmit, this.HandleError);
            OpenScriptCmd = new Command<SearchFilterResultVM>(OpenScript, () => true, HandleError);
            InitDefaults();
        }

        private void OpenScript(SearchFilterResultVM arg)
        {
            _serviceCacheIntegration.OpenScriptInNewWindow(arg.SearchResult.QueryItem.Query);
        }

        private bool CanExecuteSubmit()
        {
            return !IsLoading;
        }

        private async Task OnViewLoadedAsync()
        {
            if (_loadedOnce)
                return;

            await RequestItemsCommand.ExecuteAsync();
            _loadedOnce = true;
        }

        private async Task ExecuteRequestItemsAsync()
        {
            try
            {
                IsLoading = true;

                var filterContext = new FilterContext(QueryFilter, ServerFilter, DbFilter, StartDate.ToUniversalTime(), EndDate.ToUniversalTime());
                var result = await _itemsRepository.FindItems(filterContext);
                QueryItemsVM = result.Select(q => new ScriptSearchTarget(q)).Select(p => new SearchFilterResultVM(p, filterContext)).ToList();
    
                Message = $"{QueryItemsVM.Count} Result(s).";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void HandleError(Exception ex)
        {
            Message = ex.Message;
        }

        private void InitDefaults()
        {
            _endDate = DateTime.Now.AddDays(1).Date;
            _startDate = EndDate.AddDays(-60).Date;
        }

        private List<SearchFilterResultVM> _queryItemsVM;
        public List<SearchFilterResultVM> QueryItemsVM
        {
            get => _queryItemsVM;
            private set => SetField(ref _queryItemsVM, value);
        }

        private string _message = "Click on the button to load data";
        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetField(ref _startDate, value);
                OnSearchChange();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetField(ref _endDate, value);
                OnSearchChange();
            }
        }

        private string _queryFilter;
        public string QueryFilter
        {
            get => _queryFilter;
            set
            {
                SetField(ref _queryFilter, value);
                OnSearchChange();
            }
        }

        private string _dbFilter;
        public string DbFilter
        {
            get => _dbFilter;
            set
            {
                SetField(ref _dbFilter, value);
                OnSearchChange();
            }
        }

        private string _serverFilter;
        public string ServerFilter
        {
            get => _serverFilter;
            set
            {
                SetField(ref _serverFilter, value);
                OnSearchChange();
            }
        }

        private SearchFilterResultVM _selectedItem;
        public SearchFilterResultVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetField(ref _selectedItem, value);
            }
        }

        private void OnSearchChange()
        {
            RequestItemsCommand.ExecuteAsync();
        }
    }
}
