using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNet.CodeStandard.NamingRules;
using System.Threading.Tasks;
using VerifyCS = MoreNet.CodeStandard.Test.CSharpCodeFixVerifier<
    MoreNet.CodeStandard.NamingRules.MN1300AsyncMethodShouldEndWithAsync,
    MoreNet.CodeStandard.CodeFixes.NamingRules.MN1300CodeFixProvider>;

namespace MoreNet.CodeStandard.Test.NamingRules
{
    [TestClass]
    public class MN1300Tests
    {
        private const string Name = "Name";
        private const string NameAsync = "NameAsync";
        private const string AsyncName = "AsyncName";
        private const string AsyncNameAsync = "AsyncNameAsync";
        private const string NameAsyncName = "NameAsyncName";
        private const string NameAsyncNameAsync = "NameAsyncNameAsync";

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidMethod_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
class Foo
{{
    void Valid()
    {{
    }}

    Task ValidTask()
    {{
        return default;
    }}

    async Task ValidAsync()
    {{
    }}

    async Task {{|#0:{stubName}|}}()
    {{
    }}
}}
";

            var stubFixedCode = $@"
using System.Threading.Tasks;
class Foo
{{
    void Valid()
    {{
    }}

    Task ValidTask()
    {{
        return default;
    }}

    async Task ValidAsync()
    {{
    }}

    async Task {stubFixedName}()
    {{
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInheritanceAbstract_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal abstract void Valid();

    internal abstract Task ValidTask();

    internal abstract Task ValidAsync();

    internal abstract Task {stubName}();
}}

class Foo : FooBase
{{
    internal override void Valid()
    {{
    }}

    internal override Task ValidTask()
    {{
        return default;
    }}

    internal async override Task ValidAsync()
    {{
    }}

    internal async override Task {{|#0:{stubName}|}}()
    {{
    }}
}}
";

            var stubFixedCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal abstract void Valid();

    internal abstract Task ValidTask();

    internal abstract Task ValidAsync();

    internal abstract Task {stubFixedName}();
}}

class Foo : FooBase
{{
    internal override void Valid()
    {{
    }}

    internal override Task ValidTask()
    {{
        return default;
    }}

    internal async override Task ValidAsync()
    {{
    }}

    internal async override Task {stubFixedName}()
    {{
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInheritanceVirtual_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal virtual void Valid()
    {{
    }}

    internal virtual Task ValidTask()
    {{
        return default;
    }}

    internal async virtual Task ValidAsync()
    {{
    }}

    internal async virtual Task {{|#0:{stubName}|}}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal override void Valid()
    {{
    }}

    internal override Task ValidTask()
    {{
        return default;
    }}

    internal async override Task ValidAsync()
    {{
    }}

    internal async override Task {{|#1:{stubName}|}}()
    {{
    }}
}}
";

            var stubFixedCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal virtual void Valid()
    {{
    }}

    internal virtual Task ValidTask()
    {{
        return default;
    }}

    internal async virtual Task ValidAsync()
    {{
    }}

    internal async virtual Task {stubFixedName}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal override void Valid()
    {{
    }}

    internal override Task ValidTask()
    {{
        return default;
    }}

    internal async override Task ValidAsync()
    {{
    }}

    internal async override Task {stubFixedName}()
    {{
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
                GetBaseDiagnostic().WithLocation(1).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInheritanceNew_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {{|#0:{stubName}|}}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal new void Valid()
    {{
    }}

    internal new Task ValidTask()
    {{
        return default;
    }}

    internal async new Task ValidAsync()
    {{
    }}

    internal async new Task {{|#1:{stubName}|}}()
    {{
    }}
}}
";

            var stubFixedCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {stubFixedName}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal new void Valid()
    {{
    }}

    internal new Task ValidTask()
    {{
        return default;
    }}

    internal async new Task ValidAsync()
    {{
    }}

    internal async new Task {stubFixedName}()
    {{
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
                GetBaseDiagnostic().WithLocation(1).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInheritanceImplicitlyNew_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {{|#0:{stubName}|}}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {{|#1:{stubName}|}}()
    {{
    }}
}}
";

            var stubFixedCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {stubFixedName}()
    {{
    }}
}}

class Foo : FooBase
{{
    internal void Valid()
    {{
    }}

    internal Task ValidTask()
    {{
        return default;
    }}

    internal async Task ValidAsync()
    {{
    }}

    internal async Task {stubFixedName}()
    {{
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
                GetBaseDiagnostic().WithLocation(1).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidProperty_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action Valid {{ get; set; }}
    Func<Task> ValidTask {{ get; set; }}
    Func<Task> ValidAsync {{ get; set; }}
    Func<Task> {stubName} {{ get; set; }}

    async Task BarAsync()
    {{
        Valid = () => {{ }};
        ValidTask = () => Task.Delay(2);
        ValidAsync = async () => await Task.Delay(2);
        {{|#0:{stubName}|}} = async () => await Task.Delay(2);
    }}
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action Valid {{ get; set; }}
    Func<Task> ValidTask {{ get; set; }}
    Func<Task> ValidAsync {{ get; set; }}
    Func<Task> {stubFixedName} {{ get; set; }}

    async Task BarAsync()
    {{
        Valid = () => {{ }};
        ValidTask = () => Task.Delay(2);
        ValidAsync = async () => await Task.Delay(2);
        {stubFixedName} = async () => await Task.Delay(2);
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInitProperty_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action Valid {{ get; set; }} = () => {{ }};
    Func<Task> ValidTask {{ get; set; }} = () => Task.Delay(1);
    Func<Task> ValidAsync {{ get; set; }} = async () => await Task.Delay(1);
    Func<Task> {{|#0:{stubName}|}} {{ get; set; }} = async () => await Task.Delay(1);
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action Valid {{ get; set; }} = () => {{ }};
    Func<Task> ValidTask {{ get; set; }} = () => Task.Delay(1);
    Func<Task> ValidAsync {{ get; set; }} = async () => await Task.Delay(1);
    Func<Task> {stubFixedName} {{ get; set; }} = async () => await Task.Delay(1);
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidField_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action _valid;
    Func<Task> _validTask;
    Func<Task> _validAsync;
    Func<Task> {stubName};

    async Task BarAsync()
    {{
        _valid = () => {{ }};
        _validTask = () => Task.Delay(2);
        _validAsync = async () => await Task.Delay(2);
        {{|#0:{stubName}|}} = async () => await Task.Delay(2);
    }}
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action _valid;
    Func<Task> _validTask;
    Func<Task> _validAsync;
    Func<Task> {stubFixedName};

    async Task BarAsync()
    {{
        _valid = () => {{ }};
        _validTask = () => Task.Delay(2);
        _validAsync = async () => await Task.Delay(2);
        {stubFixedName} = async () => await Task.Delay(2);
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidInitField_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action _valid = () => {{ }};
    Func<Task> _validTask = () => Task.Delay(1);
    Func<Task> _validAsync = async () => await Task.Delay(1);
    Func<Task> {{|#0:{stubName}|}} = async () => await Task.Delay(1);
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    Action _valid = () => {{ }};
    Func<Task> _validTask = () => Task.Delay(1);
    Func<Task> _validAsync = async () => await Task.Delay(1);
    Func<Task> {stubFixedName} = async () => await Task.Delay(1);
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidLocalVariable_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action valid = () => {{ }};
        Func<Task> validTask = () => Task.Delay(2);
        Func<Task> validAsync = async () => await Task.Delay(2);
        Func<Task> {{|#0:{stubName}|}} = async () => await Task.Delay(2);
    }}
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action valid = () => {{ }};
        Func<Task> validTask = () => Task.Delay(2);
        Func<Task> validAsync = async () => await Task.Delay(2);
        Func<Task> {stubFixedName} = async () => await Task.Delay(2);
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidParenthesizedLambda_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action<string> valid = (s) => {{ }};
        Func<string, Task> validTask = (s) => Task.Delay(1);
        Func<string, Task> validAsync = async (s) => await Task.Delay(1);
        Func<string, Task> {{|#0:{stubName}|}} = async (s) => await Task.Delay(1);
    }}
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action<string> valid = (s) => {{ }};
        Func<string, Task> validTask = (s) => Task.Delay(1);
        Func<string, Task> validAsync = async (s) => await Task.Delay(1);
        Func<string, Task> {stubFixedName} = async (s) => await Task.Delay(1);
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(Name, NameAsync)]
        [DataRow(AsyncName, AsyncNameAsync)]
        [DataRow(NameAsyncName, NameAsyncNameAsync)]
        public async Task MN1300Test_ValidAndInvalidSimpleLambda_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action<string> valid = s => {{ }};
        Func<string, Task> validTask = s => Task.Delay(1);
        Func<string, Task> validAsync = async s => await Task.Delay(1);
        Func<string, Task> {{|#0:{stubName}|}} = async s => await Task.Delay(1);
    }}
}}
";

            var stubFixedCode = $@"
using System;
using System.Threading.Tasks;
class Foo
{{
    async Task BarAsync()
    {{
        Action<string> valid = s => {{ }};
        Func<string, Task> validTask = s => Task.Delay(1);
        Func<string, Task> validAsync = async s => await Task.Delay(1);
        Func<string, Task> {stubFixedName} = async s => await Task.Delay(1);
    }}
}}
";

            var allExpected = new[]
            {
                GetBaseDiagnostic().WithLocation(0).WithArguments(stubName),
            };

            await VerifyCS.VerifyCodeFixAsync(stubCode, allExpected, stubFixedCode);
        }

        private static DiagnosticResult GetBaseDiagnostic()
        {
            return VerifyCS.Diagnostic(MN1300AsyncMethodShouldEndWithAsync.DiagnosticId);
        }
    }
}
