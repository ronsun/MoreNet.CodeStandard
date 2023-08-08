using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace MoreNet.CodeStandard.NamingRules
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MN1301NonAsyncMethodMustNotEndWithAsync : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MN1301";
        private const string TailingAsync = "Async";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.MN1301Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.MN1301MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.MN1301Description), NamingResources.ResourceManager, typeof(NamingResources));

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                DiagnosticId,
                Title,
                MessageFormat,
                CategoryName.NamingRule,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description,
                helpLinkUri: HelperLinkUri.MN1301);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
            context.RegisterSyntaxNodeAction(
                AnalyzeLambdaSyntax,
                SyntaxKind.ParenthesizedLambdaExpression,
                SyntaxKind.SimpleLambdaExpression);
        }

        private void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;
            // Ignore abstract method, as the validation should works for methods implemented in derived classes.
            // This is because the "async" modifier can only be used in methods that have a body.
            // Applying this rule to abstract moethd could result in false negative.
            if (method.IsAbstract)
            {
                return;
            }

            // Ignore getter and setter methods, as the validation should works for lambda assignment.
            // This is because the "async" modifier should be applied to lamba.
            // Applying this rule to getter and setter moethds could result in false negative.
            if (method.MethodKind == MethodKind.PropertyGet
                || method.MethodKind == MethodKind.PropertySet)
            {
                return;
            }

            if (!method.IsAsync && method.Name.EndsWith(TailingAsync))
            {
                var diagnostic = Diagnostic.Create(Rule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeLambdaSyntax(SyntaxNodeAnalysisContext context)
        {
            // Ignore if the lambda expression contains the "async" keyword.
            var lambdaExpression = context.Node as LambdaExpressionSyntax;
            if (lambdaExpression.AsyncKeyword != default)
            {
                return;
            }

            if (context.Node.Parent is AssignmentExpressionSyntax assignmentExpression)
            {
                AnalyzeAssigmentWithLambda(assignmentExpression, context);
            }
            else if (context.Node.Parent is EqualsValueClauseSyntax equalsValueClauseSyntax)
            {
                if (equalsValueClauseSyntax.Parent is PropertyDeclarationSyntax propertyDeclarationSyntax)
                {
                    AnalyzePropertyInitialzationWithLambda(propertyDeclarationSyntax, context);
                }
                else if (equalsValueClauseSyntax.Parent is VariableDeclaratorSyntax variableDeclaratorSyntax)
                {
                    AnalyzeVariableDeclaratiorWithLambda(variableDeclaratorSyntax, context);
                }
            }
        }

        private void AnalyzeAssigmentWithLambda(AssignmentExpressionSyntax syntaxNode, SyntaxNodeAnalysisContext context)
        {
            if (syntaxNode.Left is IdentifierNameSyntax leftIdentifier)
            {
                var identifier = leftIdentifier.Identifier;
                AnalyzeSyntaxTokenNaming(identifier.Text, identifier.GetLocation(), context);
            }
        }

        private void AnalyzePropertyInitialzationWithLambda(PropertyDeclarationSyntax syntaxNode, SyntaxNodeAnalysisContext context)
        {
            var identifier = syntaxNode.Identifier;
            AnalyzeSyntaxTokenNaming(identifier.Text, identifier.GetLocation(), context);
        }

        private void AnalyzeVariableDeclaratiorWithLambda(VariableDeclaratorSyntax syntaxNode, SyntaxNodeAnalysisContext context)
        {
            var identifier = syntaxNode.Identifier;
            AnalyzeSyntaxTokenNaming(identifier.Text, identifier.GetLocation(), context);
        }

        private void AnalyzeSyntaxTokenNaming(string name, Location location, SyntaxNodeAnalysisContext context)
        {
            if (name.EndsWith(TailingAsync))
            {
                var diagnostic = Diagnostic.Create(Rule, location, name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
