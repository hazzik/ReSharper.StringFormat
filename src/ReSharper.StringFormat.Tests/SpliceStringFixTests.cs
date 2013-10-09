using JetBrains.ReSharper.Intentions.CSharp.QuickFixes.Tests;
using NUnit.Framework;

namespace ReSharper.StringFormat.Tests
{
    [TestFixture]
    public class SpliceStringFixTests : CSharpQuickFixTestBase<SpliceStringFix>
    {
        protected override string RelativeTestDataPath
        {
            get { return "SpliceStringAction"; }
        }

        [TestCase("execute01.cs")]
        //[TestCase("execute02.cs")]
        [TestCase("execute03.cs")]
        [TestCase("execute04.cs")]
        [TestCase("execute05.cs")]
        public void ExecuteTest(string file)
        {
            DoTestFiles(file);
        }
    }
}
