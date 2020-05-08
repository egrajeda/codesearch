using System;
using System.Collections.Generic;

namespace CodeSearch.UI
{
    public class SearchResult : IEquatable<SearchResult>
    {
        public string Uri { get; set; }

        public string Repository { get; set; }

        public string Filepath { get; set; }

        public string Filename { get; set; }

        public int LineNumber { get; set; }

        public string Code { get; set; }

        public SearchResult(string uri, string repository, string filepath, string filename, int lineNumber, string code)
        {
            Uri = uri;
            Repository = repository;
            Filepath = filepath;
            Filename = filename;
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
                   Uri == other.Uri &&
                   Repository == other.Repository &&
                   Filepath == other.Filepath &&
                   Filename == other.Filename &&
                   LineNumber == other.LineNumber &&
                   Code == other.Code;
        }

        public override int GetHashCode()
        {
            var hashCode = -1550603957;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Uri);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Repository);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filepath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filename);
            hashCode = hashCode * -1521134295 + LineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            return hashCode;
        }
    }
}