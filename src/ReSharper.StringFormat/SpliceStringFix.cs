using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.StringFormat
{
    [QuickFix]
    public class SpliceStringFix : QuickFixBase
    {
        private readonly ILiteralExpression _literalExpression;
        private readonly string _replacement;

        public SpliceStringFix(SpliceStringWarning warning)
        {
            _literalExpression = warning.LiteralExpression;
            _replacement = warning.Replacement;
        }

        public override string Text
        {
            get { return "Replace with " + _replacement; }
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _literalExpression != null && _literalExpression.IsValid();
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = CSharpElementFactory.GetInstance(_literalExpression);
            var newExpr = factory.CreateExpressionAsIs(_replacement);
            _literalExpression.ReplaceBy(newExpr);
            return null;
        }
    }
}
