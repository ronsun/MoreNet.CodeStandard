using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreNet.CodeStandard.MaintainabilityRules;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNet.CodeStandard.CodeFixes.MaintainabilityRules
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MN1400CodeFixProvider)), Shared]
    public class MN1400CodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(MN1400PlaceStringInterpolationBeforeVerbatimText.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var interpolatedString = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InterpolatedStringExpressionSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    MaintainabilityResources.MN1400CodeFix,
                    token => ReplaceAsync(context.Document, interpolatedString, token)),
                diagnostic);
        }

        private async Task<Document> ReplaceAsync(Document document, InterpolatedStringExpressionSyntax interpolatedString, CancellationToken cancellationToken)
        {
            var oldTokenText = interpolatedString.StringStartToken.Text;
            var newTokenText = oldTokenText.Replace("@$", "$@");

            var newToken = SyntaxFactory.Token(
                interpolatedString.StringStartToken.LeadingTrivia,
                SyntaxKind.InterpolatedVerbatimStringStartToken,
                newTokenText,
                newTokenText,
                interpolatedString.StringStartToken.TrailingTrivia);

            var newInterpolatedString = interpolatedString.WithStringStartToken(newToken);

            // Replace the old interpolated string with the new one
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(interpolatedString, newInterpolatedString);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
