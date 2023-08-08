using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNet.CodeStandard.MaintainabilityRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyCS = MoreNet.CodeStandard.Test.CSharpCodeFixVerifier<
    MoreNet.CodeStandard.MaintainabilityRules.MN1400PlaceStringInterpolationBeforeVerbatimText,
    MoreNet.CodeStandard.CodeFixes.MaintainabilityRules.MN1400CodeFixProvider>;

namespace MoreNet.CodeStandard.Test.MaintainabilityRules
{
    [TestClass]
    public class MN1400Tests
    {
        [TestMethod]
        [DynamicData(nameof(MN1400Test_NoDiagnosticsTestCaseSource), DynamicDataSourceType.Method)]
        public async Task MN1400Test_NoDiagnostics(string stubCode)
        {
            await VerifyCS.VerifyAnalyzerAsync(stubCode);
        }

        private static IEnumerable<object[]> MN1400Test_NoDiagnosticsTestCaseSource()
        {
            string stubCode;

            // Local Variable
            stubCode = @"
class Foo
{
    void Method()
    {
        string s1 = $@""v1"";
    }
}
";
            yield return new object[] { stubCode };
        }

        [TestMethod]
        [DynamicData(nameof(MN1400Test_ExpectedDiagnosticsAndFixesTestCaseSource), DynamicDataSourceType.Method)]
        public async Task MN1400Test_ExpectedDiagnosticsAndFixes(
            string stubCode,
            DiagnosticResult[] allExpected,
            string stubFixedCode)
        {
            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        private static IEnumerable<object[]> MN1400Test_ExpectedDiagnosticsAndFixesTestCaseSource()
        {
            string stubCode = default;
            Func<string> stubFixedCodeBuilder = () => stubCode.Replace("@$", "$@");
            DiagnosticResult[] allExpected;

            // Local Variable - vary formats.
            stubCode = @"
class Foo
{
    void Method()
    {
        string s1 = {|#0:@$""value""|};
        string s2 = {|#1:@$""va-
lue""|};
        var s3 = {|#2:@$""value""|};
        int i1 = 1;
        var i2 = 2;
        string s4 = {|#3:@$""value""|} + {|#4:@$""value""|};
    }
}
";
            allExpected =
                new[]
                {
                    GetBaseDiagnostic().WithLocation(0),
                    GetBaseDiagnostic().WithLocation(1),
                    GetBaseDiagnostic().WithLocation(2),
                    GetBaseDiagnostic().WithLocation(3),
                    GetBaseDiagnostic().WithLocation(4),
                };

            yield return new object[] { stubCode, allExpected, stubFixedCodeBuilder.Invoke() };

            // Local Variable - vary positions.
            stubCode = @"
using System;
class Foo
{
    string _f = {|#0:@$""value""|};

    string P { get; set; } = {|#1:@$""value""|};

    Foo()
    {
        var s = {|#2:@$""value""|};
    }

    void Method()
    {
        var s = {|#3:@$""value""|};
    }

    static void StaticMethod()
    {
        var s = {|#4:@$""value""|};
    }
    
    void Caller()
    {
        LocalFunction1({|#5:@$""value""|});
        Callee({|#6:@$""value""|});

        void LocalFunction1(string s)
        {
        }
    }

    void Callee(string s)
    {
    }

    void Lambda()
    {
        Func<string> func = () => {|#7:@$""value""|};
    }

    void ChangeDefault()
    {
        _f = {|#8:@$""value""|};
        P = {|#9:@$""value""|};
    }
}
";

            allExpected =
                new[]
                {
                    GetBaseDiagnostic().WithLocation(0),
                    GetBaseDiagnostic().WithLocation(1),
                    GetBaseDiagnostic().WithLocation(2),
                    GetBaseDiagnostic().WithLocation(3),
                    GetBaseDiagnostic().WithLocation(4),
                    GetBaseDiagnostic().WithLocation(5),
                    GetBaseDiagnostic().WithLocation(6),
                    GetBaseDiagnostic().WithLocation(7),
                    GetBaseDiagnostic().WithLocation(8),
                    GetBaseDiagnostic().WithLocation(9),
                };
            yield return new object[] { stubCode, allExpected, stubFixedCodeBuilder.Invoke() };

            // Local Variable - vary assignments
            stubCode = @"
class Foo
{
    string _f;

    string P { get; set; }

    void Method()
    {
        _f = {|#0:@$""value""|};
        P = {|#1:@$""value""|};
    }
}
            ";
            allExpected =
                new[]
                {
                    GetBaseDiagnostic().WithLocation(0),
                    GetBaseDiagnostic().WithLocation(1),
                };
            yield return new object[] { stubCode, allExpected, stubFixedCodeBuilder.Invoke() };
        }

        private static DiagnosticResult GetBaseDiagnostic()
        {
            return VerifyCS.Diagnostic(MN1400PlaceStringInterpolationBeforeVerbatimText.DiagnosticId);
        }
    }
}
