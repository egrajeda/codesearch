using System;
using System.Collections.Generic;

namespace CodeSearch.Core
{
    public class SearchResults
    {
        public IEnumerable<SearchResult> Results { get; }

        public SearchResults(IEnumerable<SearchResult> results)
        {
            Results = results;
        }
    }

    public class SearchResult : IEquatable<SearchResult>
    {
        public Uri Uri { get; }

        public string Repository { get; }

        public string Filepath { get; }

        public int LineNumber { get; }

        public string Code { get; }

        public SearchResult(Uri uri, string repository, string filepath, int lineNumber, string code)
        {
            Uri = uri;
            Repository = repository;
            Filepath = filepath;
            LineNumber = lineNumber;
            Code = code;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SearchResult);
        }

        public bool Equals(SearchResult other)
        {
            return other != null &&
                   EqualityComparer<Uri>.Default.Equals(Uri, other.Uri) &&
                   Repository == other.Repository &&
                   Filepath == other.Filepath &&
                   LineNumber == other.LineNumber &&
                   Code == other.Code;
        }

        public override int GetHashCode()
        {
            var hashCode = -1659163720;
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(Uri);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Repository);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filepath);
            hashCode = hashCode * -1521134295 + LineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            return hashCode;
        }
    }
}
