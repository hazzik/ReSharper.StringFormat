using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.Special;

namespace ReSharper.StringFormat
{
    [ElementProblemAnalyzer(typeof (ILiteralExpression), 
        HighlightingTypes = new[] { typeof (SpliceStringWarning) })]
    public class StringFormatAnalyzer : ElementProblemAnalyzer<ILiteralExpression>
    {
        private static readonly Regex regex = new Regex(@"\{([^{:}]+)(:[^}]+)?\}", RegexOptions.Compiled);

        protected override void Run(ILiteralExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.IsValid() && element.IsConstantValue() && element.ConstantValue.IsString() && IsNotInAttribute(element))
            {
                var str = element.GetText();
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
                            var replacement = string.Format("string.Format({0}, {1})",
                                arguments.Aggregate(str, (current, argument) => argument.Replace(current)),
                                string.Join(", ", arguments));

                            consumer.AddHighlighting(new SpliceStringWarning(element, replacement), element.GetHighlightingRange());
                        }
                    }
                }
            }
        }

        private static bool IsNotInAttribute(ITreeNode element)
        {
            return !GeneralUtil.ParentReversedPath(element, x => x.Parent).OfType<IAttribute>().Any();
        }

        private static bool IsNumber(string x)
        {
            int number;
            return int.TryParse(x, out number);
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
