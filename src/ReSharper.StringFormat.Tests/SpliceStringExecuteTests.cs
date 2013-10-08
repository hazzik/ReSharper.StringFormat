using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.CSharp.Test;
using JetBrains.ReSharper.Intentions.Extensibility;
using NUnit.Framework;

namespace ReSharper.StringFormat.Tests
{
    [TestFixture]
    public class SpliceStringExecuteTests : CSharpContextActionExecuteTestBase
    {
        protected override string ExtraPath
        {
            get { return "SpliceStringAction"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return "SpliceStringAction"; }
        }

        protected override IContextAction CreateContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            return new SpliceStringAction(dataProvider);
        }

        [TestCase("execute01.cs")]
        [TestCase("execute02.cs")]
        [TestCase("execute03.cs")]
        [TestCase("execute04.cs")]
        [TestCase("execute05.cs")]
        public void ExecuteTest(string file)
        {
            DoTestFiles(file);
        }
    }
}
