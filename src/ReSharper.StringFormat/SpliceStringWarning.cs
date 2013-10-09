using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.StringFormat
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class SpliceStringWarning : IHighlighting
    {
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