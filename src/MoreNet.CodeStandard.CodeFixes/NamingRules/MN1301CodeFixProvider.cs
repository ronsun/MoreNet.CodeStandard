using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using MoreNet.CodeStandard.NamingRules;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoreNet.CodeStandard.CodeFixes.NamingRules
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MN1301CodeFixProvider)), Shared]
    public class MN1301CodeFixProvider : CodeFixProvider
    {
        private const string TailingAsync = "Async";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(MN1301NonAsyncMethodMustNotEndWithAsync.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var identifierToken = root.FindToken(diagnosticSpan.Start);

            if (identifierToken.Parent is MethodDeclarationSyntax methodDeclaration)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        NamingResources.MN1301CodeFix,
                        token => RemoveTailingAsyncFromMethodName(context.Document, methodDeclaration, token),
                        nameof(MN1301CodeFixProvider)),
                    diagnostic);
            }
            else
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        NamingResources.MN1301CodeFix,
                        token => RemoveTailingAsyncFromIdentifierName(context.Document, identifierToken, token),
                        nameof(MN1301CodeFixProvider)),
                    diagnostic);
            }
        }

        private async Task<Solution> RemoveTailingAsyncFromMethodName(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            var originalSolution = document.Project.Solution;
            var options = originalSolution.Workspace.Options;

            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken);

            var newMethodName = RemoveTailingAsync(methodDeclaration.Identifier.Text);
            var newSolution = await Renamer.RenameSymbolAsync(originalSolution, methodSymbol, newMethodName, options, cancellationToken).ConfigureAwait(false);
            return newSolution;
        }

        private async Task<Solution> RemoveTailingAsyncFromIdentifierName(Document document, SyntaxToken identifierToken, CancellationToken cancellationToken)
        {
            var originalSolution = document.Project.Solution;
            var options = originalSolution.Workspace.Options;

            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var node = identifierToken.Parent;

            ISymbol symbolToRename = node switch
            {
                // Including field initialization, property initialization and local variable declaration.
                VariableDeclaratorSyntax variableDeclaratorSyntax => semanticModel.GetDeclaredSymbol(variableDeclaratorSyntax, cancellationToken),
                PropertyDeclarationSyntax propertyDeclarationSyntax => semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax, cancellationToken),
                IdentifierNameSyntax identifierNameSyntax
                    when identifierNameSyntax.Parent is AssignmentExpressionSyntax assignmentExpressionSyntax
                    => semanticModel.GetSymbolInfo(assignmentExpressionSyntax.Left, cancellationToken).Symbol,
                _ => null,
            };

            if (symbolToRename != null)
            {
                var newName = RemoveTailingAsync(symbolToRename.Name);
                var newSolution = await Renamer.RenameSymbolAsync(originalSolution, symbolToRename, newName, options, cancellationToken).ConfigureAwait(false);
                return newSolution;
            }

            return originalSolution;
        }

        private string RemoveTailingAsync(string name)
        {
            while (name.EndsWith(TailingAsync, StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - 5);
            }

            return name;
        }
    }
}
