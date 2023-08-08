using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNet.CodeStandard.NamingRules;
using System.Threading.Tasks;
using VerifyCS = MoreNet.CodeStandard.Test.CSharpCodeFixVerifier<
    MoreNet.CodeStandard.NamingRules.MN1301NonAsyncMethodMustNotEndWithAsync,
    MoreNet.CodeStandard.CodeFixes.NamingRules.MN1301CodeFixProvider>;

namespace MoreNet.CodeStandard.Test.NamingRules
{
    [TestClass]
    public class MN1301Tests
    {
        private const string Name = "Name";
        private const string NameAsync = "NameAsync";
        private const string NameAsyncAsync = "NameAsyncAsync";
        private const string AsyncName = "AsyncName";
        private const string AsyncNameAsync = "AsyncNameAsync";
        private const string NameAsyncName = "NameAsyncName";
        private const string NameAsyncNameAsync = "NameAsyncNameAsync";

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
        public async Task MN1301Test_ValidAndInvalidMethod_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
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

    void {{|#0:{stubName}|}}()
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

    void {stubFixedName}()
    {{
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
        public async Task MN1300Test_ValidAndInvalidInheritanceAbstract_ExpectedDiagnosticsAndFixes(string stubName, string stubFixedName)
        {
            var stubCode = $@"
using System.Threading.Tasks;
abstract class FooBase
{{
    internal abstract void Valid();

    internal abstract Task ValidTask();

    internal abstract Task ValidAsync();

    internal abstract void {stubName}();
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

    internal override void {{|#0:{stubName}|}}()
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

    internal abstract void {stubFixedName}();
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

    internal override void {stubFixedName}()
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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

    internal virtual void {{|#0:{stubName}|}}()
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

    internal override void {{|#1:{stubName}|}}()
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

    internal virtual void {stubFixedName}()
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

    internal override void {stubFixedName}()
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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

    internal void {{|#0:{stubName}|}}()
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

    internal new void {{|#1:{stubName}|}}()
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

    internal void {stubFixedName}()
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

    internal new void {stubFixedName}()
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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

    internal void {{|#0:{stubName}|}}()
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

    internal void {{|#1:{stubName}|}}()
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

    internal void {stubFixedName}()
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

    internal void {stubFixedName}()
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
    Action {stubName} {{ get; set; }}

    async Task BarAsync()
    {{
        Valid = () => {{ }};
        ValidTask = () => Task.Delay(2);
        ValidAsync = async () => await Task.Delay(2);
        {{|#0:{stubName}|}} = () => {{ }};
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
    Action {stubFixedName} {{ get; set; }}

    async Task BarAsync()
    {{
        Valid = () => {{ }};
        ValidTask = () => Task.Delay(2);
        ValidAsync = async () => await Task.Delay(2);
        {stubFixedName} = () => {{ }};
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
    Action {{|#0:{stubName}|}} {{ get; set; }} = () => {{ }};
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
    Action {stubFixedName} {{ get; set; }} = () => {{ }};
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
    Action {stubName};

    async Task BarAsync()
    {{
        _valid = () => {{ }};
        _validTask = () => Task.Delay(2);
        _validAsync = async () => await Task.Delay(2);
        {{|#0:{stubName}|}} = () => {{ }};
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
    Action {stubFixedName};

    async Task BarAsync()
    {{
        _valid = () => {{ }};
        _validTask = () => Task.Delay(2);
        _validAsync = async () => await Task.Delay(2);
        {stubFixedName} = () => {{ }};
    }}
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
    Action {{|#0:{stubName}|}} = () => {{ }};
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
    Action {stubFixedName} = () => {{ }};
}}
";

            var expected = GetBaseDiagnostic().WithLocation(0).WithArguments(stubName);

            await VerifyCS.VerifyCodeFixAsync(stubCode, expected, stubFixedCode);
        }

        [TestMethod]
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
        Action {{|#0:{stubName}|}} = () => {{}};
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
        Action {stubFixedName} = () => {{}};
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
        Action<string> {{|#0:{stubName}|}} = (s) => {{}};
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
        Action<string> {stubFixedName} = (s) => {{}};
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
        [DataRow(NameAsync, Name)]
        [DataRow(NameAsyncAsync, Name)]
        [DataRow(AsyncNameAsync, AsyncName)]
        [DataRow(NameAsyncNameAsync, NameAsyncName)]
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
        Action<string> {{|#0:{stubName}|}} = s => {{}};
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
        Action<string> {stubFixedName} = s => {{}};
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
            return VerifyCS.Diagnostic(MN1301NonAsyncMethodMustNotEndWithAsync.DiagnosticId);
        }
    }
}
