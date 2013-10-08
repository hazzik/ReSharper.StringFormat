using System;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.StringFormat
{
    [ContextAction(Name = "SpliceString", Description = "Splices a string", Group = "C#")]
    public class SpliceStringAction : ContextActionBase
    {
        private static readonly Regex regex = new Regex(@"\{([\w]+)\}", RegexOptions.Compiled);

        private readonly ICSharpContextActionDataProvider _provider;
        private ILiteralExpression _target;
        private string _replacement;

        public SpliceStringAction(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override string Text
        {
            get { return "Replace with " + _replacement; }
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            var literal = _provider.GetSelectedElement<ILiteralExpression>(true, true);
            if (literal != null && literal.IsConstantValue() && literal.ConstantValue.IsString())
            {
                var str = literal.ConstantValue.Value as string;
                if (!string.IsNullOrEmpty(str))
                {
                    var matches = regex.Matches(str);
                    if (matches.Count > 0)
                    {
                        var arguments = matches
                            .Cast<Match>()
                            .Select(m => m.Groups[1].Value)
                            .Where(s => !IsNumber(s))
                            .Distinct()
                            .ToArray();

                        if (arguments.Length > 0)
                        {
                            for (int i = 0; i < arguments.Length; i++)
                            {
                                var argument = arguments[i];
                                str = str.Replace("{" + argument + "}", "{" + i + "}");
                            }

                            _replacement = "string.Format(\"" + StringLiteralConverter.EscapeToRegular(str) + "\", " + string.Join(", ", arguments) + ")";
                            _target = literal;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool IsNumber(string x)
        {
            int number;
            return int.TryParse(x, out number);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = CSharpElementFactory.GetInstance(_provider.PsiModule);
            var newExpr = factory.CreateExpressionAsIs(_replacement);
            _target.ReplaceBy(newExpr);
            return null;
        }
    }
}
