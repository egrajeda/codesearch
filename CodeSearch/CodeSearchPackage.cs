using CodeSearch.UI;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace CodeSearch
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(CodeSearchOptions), "Code Search", "General", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(SearchResultsToolWindow))]
    public sealed class CodeSearchPackage : AsyncPackage
    {
        public const string PackageGuidString = "fa51c09c-5cc1-4892-9344-cf1cf71f6185";

        private CodeSearchOptions Options => GetDialogPage(typeof(CodeSearchOptions)) as CodeSearchOptions;

        public string SourcegraphUri => Options.SourcegraphUri;

        public string SourcegraphAccessToken => Options.SourcegraphAccessToken;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await SearchResultsToolWindowCommand.InitializeAsync(this);
            await CodeSearch.UI.SearchTokenCommand.InitializeAsync(this);
        }
    }

    public class CodeSearchOptions : DialogPage
    {
        [Category("Sourcegraph")]
        [DisplayName("URL")]
        [Description("URL to access Sourcegraph. The API should be available under <URL>/.api/graphql.")]
        public string SourcegraphUri { get; set; }

        [Category("Sourcegraph")]
        [DisplayName("Access Token")]
        [Description("Access Token used to query Sourcegraph API. You can create one in Sourcegraph settings page.")]
        public string SourcegraphAccessToken { get; set; }
    }
}
