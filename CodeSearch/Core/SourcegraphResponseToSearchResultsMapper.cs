using System;
using System.Linq;

namespace CodeSearch.Core
{
    internal static class SourcegraphResponseToSearchResultsMapper
    {
        public static SearchResults Map(Uri sourcegraphUri, SourcegraphResponse response)
        {
            return new SearchResults(
                response.Data.Search.Results.Results.SelectMany(
                    results =>
                    {
                        return results.LineMatches.Select(
                            match =>
                            {
                                return new SearchResult(
                                    new Uri(sourcegraphUri, results.File.Url),
                                    results.Repository.Name.Trim(),
                                    "/" + results.File.Path.Trim(),
                                    match.LineNumber,
                                    match.Preview.Trim());
                            });
                    }));
        }
    }
}
