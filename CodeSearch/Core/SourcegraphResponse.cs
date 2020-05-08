using System.Collections.Generic;

namespace CodeSearch.Core
{
    internal class SourcegraphResponse
    {
        public SourcegraphData Data { get; set; }
    }

    internal class SourcegraphData
    {
        public SourcegraphSearch Search { get; set; }
    }

    internal class SourcegraphSearch
    {
        public SourcegraphResultsInformation Results { get; set; }
    }

    internal class SourcegraphResultsInformation
    {
        public int MatchCount { get; set; }

        public IEnumerable<SourcegraphResult> Results { get; set; }
    }

    internal class SourcegraphResult
    {
        public SourcegraphFile File { get; set; }

        public SourcegraphRepository Repository { get; set; }

        public IEnumerable<SourcegraphLineMatch> LineMatches { get; set; }
    }

    internal class SourcegraphFile
    {
        public string Path { get; set; }

        public string Url { get; set; }
    }

    internal class SourcegraphRepository
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }

    internal class SourcegraphLineMatch
    {
        public int LineNumber { get; set; }

        public string Preview { get; set; }
    }
}