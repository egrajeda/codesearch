using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace CodeSearch.UI
{
    internal sealed class SearchTokenCommand
    {
        public const int CommandId = 256;
        public static readonly Guid CommandSet = new Guid("e4a283db-f23a-4d46-be8c-484c4e313fa6");

        public static SearchTokenCommand Instance { get; private set; }

        private readonly AsyncPackage _package;

        private SearchTokenCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new SearchTokenCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var token = await GetTokenAtCaretPositionAsync();

            ToolWindowPane window = _package.FindToolWindow(typeof(SearchResultsToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var searchResultsToolWindow = window as SearchResultsToolWindow;
            var searchResultsToolWindowFrame = window.Frame as IVsWindowFrame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(searchResultsToolWindowFrame.Show());

            var searchResultsToolWindowControl = searchResultsToolWindow.Content as SearchResultsToolWindowControl;
            searchResultsToolWindowControl.Search(token.ValueText);
        }

        private async Task<SyntaxToken> GetTokenAtCaretPositionAsync()
        {
            var textManager = await _package?.GetServiceAsync(typeof(SVsTextManager)) as IVsTextManager;
            var componentModel = await _package?.GetServiceAsync(typeof(SComponentModel)) as IComponentModel;
            var editor = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            textManager.GetActiveView(1, null, out var textView);
            var wpfTextView = editor.GetWpfTextView(textView);

            var caretPosition = wpfTextView.Caret.Position.BufferPosition;
            var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var syntaxRoot = await document.GetSyntaxRootAsync();

            return syntaxRoot.FindToken(caretPosition);
        }
    }
}