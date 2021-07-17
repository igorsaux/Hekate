using System.Text.RegularExpressions;
using ChaoticOnyx.Hekate.Server.Extensions;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server.Language.Analyzers
{
    public sealed class SpanAnalyzer : CodeAnalyzer
    {
        private readonly string _issueId     = "span/use-span-macros";
        private readonly string _htmlPattern = @"\s*""\s*<\s*span\s+class\s*=\s*['""](?<class>[^'""]*)\s*['""]\s*>(?<content>\s*[^><]*\s*)<\s*\/\s*span\s*>\s*""\s*";

        public override void OnVisit(VisitContext visitContext)
        {
            if (!visitContext.PreprocessorContext.Defines.ContainsKey("SPAN"))
            {
                return;
            }

            SyntaxToken token = visitContext.Token.Value;

            switch (token.Kind)
            {
                case SyntaxKind.TextLiteral:
                    Match match = Regex.Match(token.Text, _htmlPattern);

                    if (!match.Success)
                    {
                        break;
                    }

                    AddDiagnostic(visitContext.CodeFile, _issueId, "Используйте макрос SPAN", DiagnosticSeverity.Warning, token.GetRange());

                    break;
            }
        }
    }
}
