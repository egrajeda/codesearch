using CodeSearch.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CodeSearch.UI
{
    public partial class SearchResultsToolWindowControl : UserControl
    {
        private readonly SearchResultsToolWindow _toolWindow;

        private CodeSearchPackage Package => _toolWindow.Package as CodeSearchPackage;

        private ViewModel ViewModel => DataContext as ViewModel;

        public SearchResultsToolWindowControl(SearchResultsToolWindow toolWindow)
        {
            _toolWindow = toolWindow;
            InitializeComponent();
            DataContext = new ViewModel(() => new Sourcegraph(new Uri(Package.SourcegraphUri), Package.SourcegraphAccessToken));
        }

        public void Search(string query)
        {
            ViewModel.Query = query;
            ViewModel.Search.Execute(null);
        }

        public void ListViewItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item && item.Content is SearchResult result)
            {
                Process.Start(result.Uri);
            }
        }
    }
}