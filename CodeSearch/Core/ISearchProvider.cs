using System.Threading.Tasks;

namespace CodeSearch.Core
{
    public interface ISearchProvider
    {
        Task<SearchResults> SearchAsync(string query, Language language = Language.CSharp);
    }

    public enum Language
    {
        All,
        CSharp
    }
}
