using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.StringFormat;

[assembly: RegisterConfigurableSeverity(SpliceStringWarning.HIGHLIGHTING_ID, null, HighlightingGroupIds.CodeInfo, "Splice string is required", "Do you want to splice the string?", Severity.SUGGESTION, false)]
namespace ReSharper.StringFormat
{
    [ConfigurableSeverityHighlighting(HIGHLIGHTING_ID, CSharpLanguage.Name)]
    public class SpliceStringWarning : IHighlighting
    {
        public const string HIGHLIGHTING_ID = "SpliceString";
        private readonly ILiteralExpression _element;
        private readonly string _replacement;

        public SpliceStringWarning(ILiteralExpression element, string replacement)
        {
            _element = element;
            _replacement = replacement;
        }

        public bool IsValid()
        {
            return LiteralExpression != null && LiteralExpression.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return _element.GetDocumentRange();
        }

        public string ToolTip
        {
            get { return "Splice string is required"; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public ILiteralExpression LiteralExpression
        {
            get { return _element; }
        }

        public string Replacement
        {
            get { return _replacement; }
        }
    }
}