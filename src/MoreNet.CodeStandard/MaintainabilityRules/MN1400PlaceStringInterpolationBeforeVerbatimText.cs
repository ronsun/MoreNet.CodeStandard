using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace MoreNet.CodeStandard.MaintainabilityRules
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MN1400PlaceStringInterpolationBeforeVerbatimText : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MN1400";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(MaintainabilityResources.MN1400Title), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(MaintainabilityResources.MN1400MessageFormat), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(MaintainabilityResources.MN1400Description), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                DiagnosticId,
                Title,
                MessageFormat,
                CategoryName.MaintainabilityRules,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description,
                helpLinkUri: HelperLinkUri.MN1400);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeInterpolatedString, SyntaxKind.InterpolatedStringExpression);
        }

        private void AnalyzeInterpolatedString(SyntaxNodeAnalysisContext context)
        {
            var interpolatedString = (InterpolatedStringExpressionSyntax)context.Node;

            if (interpolatedString.StringStartToken.Text == "@$\"")
            {
                var diagnostic = Diagnostic.Create(Rule, interpolatedString.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
