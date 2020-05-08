using System.Collections.Generic;
using System.Linq;
using CoreSearchResults = CodeSearch.Core.SearchResults;
using UISearchResult = CodeSearch.UI.SearchResult;

namespace CodeSearch.UI
{
    public class CoreSearchResultsToUISearchResultsMapper
    {
        public static IEnumerable<UISearchResult> Map(CoreSearchResults coreSearchResults)
        {
            return coreSearchResults.Results.Select(
                result =>
                {
                    var pathPieces = result.Filepath.Split('/');
                    var filename = pathPieces.Last();
                    return new UISearchResult(result.Uri.ToString(), result.Repository, result.Filepath,
                        filename, result.LineNumber, result.Code);
                });
        }
    }
}