using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeSearch.Core
{
    public class Sourcegraph : ISearchProvider
    {
        private const string _searchGraphQLQuery = @"
            query ($query: String!) {
              search(query: $query) {
                results {
                  matchCount
                  results {
                    ... on FileMatch {
                      file {
                        path
                        url
                      }
                      repository {
                        name
                      }
                      lineMatches {
                        lineNumber
                        preview
                      }
                    }
                  }
                }
              }
            }";

        private readonly Uri _baseAddress;
        private readonly string _accessToken;

        public Sourcegraph(Uri baseAddress, string accessToken = null)
        {
            _baseAddress = baseAddress;
            _accessToken = accessToken;
        }

        public async Task<SearchResults> SearchAsync(string query, Language language = Language.CSharp)
        {
            using (var handler = new WebRequestHandler { ServerCertificateValidationCallback  = IgnoreServerCertificate })
            using (var client = new HttpClient(handler) { BaseAddress = _baseAddress })
            {
                if (_accessToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _accessToken);
                }

                if (language == Language.CSharp)
                {
                    query += " lang:csharp";
                }

                var request = new GraphQLRequest(_searchGraphQLQuery,
                    new Dictionary<string, string>
                    {
                        ["query"] = query
                    });
                var requestAsString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestAsString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(".api/graphql", content);
                var responseContentAsString = await response.Content.ReadAsStringAsync();

                return SourcegraphResponseToSearchResultsMapper.Map(
                    _baseAddress,
                    JsonConvert.DeserializeObject<SourcegraphResponse>(responseContentAsString));
            }
        }

        private bool IgnoreServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private class GraphQLRequest
        {
            public string Query { get; }

            public IDictionary<string, string> Variables { get; }

            public GraphQLRequest(string query, IDictionary<string, string> variables)
            {
                Query = query;
                Variables = variables;
            }
        }
    }
}
