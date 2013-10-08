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
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.StringFormat
{
    [ContextAction(Name = "SpliceString", Description = "Splices a string", Group = "C#")]
    public class SpliceStringAction : ContextActionBase
    {
        private static readonly Regex regex = new Regex(@"\{([^{:}]+)(:[^}]+)?\}", RegexOptions.Compiled);

        private readonly ICSharpContextActionDataProvider _provider;
        private string _replacement;
        private ILiteralExpression _target;

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
            if (literal != null && literal.IsValid() && literal.IsConstantValue() && literal.ConstantValue.IsString())
            {
                var str = literal.GetText();
                if (!string.IsNullOrEmpty(str))
                {
                    var matches = regex.Matches(str);
                    if (matches.Count > 0)
                    {
                        var arguments = matches
                            .Cast<Match>()
                            .GroupBy(m => m.Groups[1].Value, m => m.Groups[2].Value)
                            .Where(x => !IsNumber(x.Key))
                            .Select((x, i) => new Argument(i, x.Key, x.ToArray()))
                            .ToList();

                        if (arguments.Count > 0)
                        {
                            _replacement = string.Format("string.Format({0}, {1})",
                                arguments.Aggregate(str, (current, argument) => argument.Replace(current)),
                                string.Join(", ", arguments));

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

        private class Argument
        {
            private readonly string[] _formats;
            private readonly int _index;
            private readonly string _name;

            public Argument(int index, string name, string[] formats)
            {
                _index = index;
                _name = name;
                _formats = formats;
            }

            public string Replace(string value)
            {
                return _formats.Aggregate(value, (current, format) => current.Replace("{" + _name + format + "}", "{" + _index + format + "}"));
            }

            public override string ToString()
            {
                return _name;
            }
        }
    }
}
