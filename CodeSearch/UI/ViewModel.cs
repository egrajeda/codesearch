using CodeSearch.Core;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeSearch.UI
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        private readonly Func<ISearchProvider> _searchProviderBuilder;

        public string Query { get; set; }

        public bool OnlyCSharp { get; set; } = true;

        public ObservableCollection<SearchResult> SearchResults { get; set; } = new ObservableCollection<SearchResult>();

        public ICommand Search { get; }

        public ViewModel(Func<ISearchProvider> searchProviderBuilder)
        {
            _searchProviderBuilder = searchProviderBuilder;
            Search = new DelegateCommand(OnSearchAsync);
        }

        private async Task OnSearchAsync(object parameter)
        {
            var searchProvider = _searchProviderBuilder();
            var response = await searchProvider.SearchAsync(Query, OnlyCSharp ? Language.CSharp : Language.All);

            SearchResults = new ObservableCollection<SearchResult>(
                CoreSearchResultsToUISearchResultsMapper.Map(response));
        }

        private class DelegateCommand : ICommand
        {
            private readonly Func<object, Task> _command;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public DelegateCommand(Func<object, Task> command)
            {
                _command = command;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public async void Execute(object parameter)
            {
                await _command(parameter);
            }

            protected void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}