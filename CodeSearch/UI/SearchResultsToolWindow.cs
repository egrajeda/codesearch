using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace CodeSearch.UI
{
    [Guid("759979e2-0b85-4862-a933-04bcd74a86ed")]
    public class SearchResultsToolWindow : ToolWindowPane
    {
        public SearchResultsToolWindow() : base(null)
        {
            Caption = "Code Search";
            Content = new SearchResultsToolWindowControl(this);
        }
    }
}