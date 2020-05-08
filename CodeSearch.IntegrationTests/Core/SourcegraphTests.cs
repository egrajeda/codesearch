using NUnit.Framework;
using CodeSearch.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSearch.Tests.Core
{
    public class SourcegraphTests
    {
        private readonly ISearchProvider _searchProvider = new Sourcegraph(new Uri("https://sourcegraph.com/"));

        [Test]
        public async Task SearchesAQuery()
        {
            var results = await _searchProvider.SearchAsync("GroupRouterImpl lang:scala", Language.All);

            Assert.AreEqual(7, results.Results.Count());

            CollectionAssert.Contains(results.Results,
                new SearchResult(
                    new Uri("https://sourcegraph.com/github.com/akka/akka@8cfc23da37bcb2cd63218c6ed231bd1f8e9233a4/-/blob/akka-actor-typed-tests/src/test/scala/akka/actor/typed/scaladsl/RoutersSpec.scala"),
                    "github.com/akka/akka",
                    "/akka-actor-typed-tests/src/test/scala/akka/actor/typed/scaladsl/RoutersSpec.scala",
                    16,
                    "import akka.actor.typed.internal.routing.GroupRouterImpl"));
            CollectionAssert.Contains(results.Results,
                new SearchResult(
                    new Uri("https://sourcegraph.com/github.com/akka/akka@8cfc23da37bcb2cd63218c6ed231bd1f8e9233a4/-/blob/akka-actor-typed/src/main/scala/akka/actor/typed/internal/routing/GroupRouterImpl.scala"),
                    "github.com/akka/akka",
                    "/akka-actor-typed/src/main/scala/akka/actor/typed/internal/routing/GroupRouterImpl.scala",
                    31,
                    "new InitialGroupRouterImpl[T](ctx.asScala, key, preferLocalRoutees, logicFactory(ctx.asScala.system))"));
        }

        [Test]
        public async Task SearchesAQueryExclusivelyInCSharp()
        {
            var results = await _searchProvider.SearchAsync("HtmlTree", Language.CSharp);

            Assert.IsNotEmpty(results.Results);
            Assert.IsTrue(results.Results.All(x => x.Filepath.EndsWith(".cs")));
        }
    }
}