using System.Collections.Generic;
using System.IO;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public abstract class CodeAnalyzer : SyntaxWalker
    {
        public Dictionary<string, List<Diagnostic>> Diagnostics { get; } = new();

        internal void AddDiagnostic(string filePath, string id, string message, DiagnosticSeverity severity, Range range)
        {
            var diagnostic = new Diagnostic
            {
                Code = id, Message = message, Severity = severity, Range = range
            };

            if (Diagnostics.ContainsKey(filePath))
            {
                Diagnostics[filePath]
                    .Add(diagnostic);
            }
            else
            {
                Diagnostics.Add(filePath, new List<Diagnostic>
                {
                    diagnostic
                });
            }
        }

        internal void AddDiagnostic(CodeFile codeFile, string id, string message, DiagnosticSeverity severity, Range range)
        {
            AddDiagnostic(codeFile.File.FullName, id, message, severity, range);
        }

        internal void AddDiagnostic(FileInfo fileInfo, string id, string message, DiagnosticSeverity severity, Range range)
        {
            AddDiagnostic(fileInfo.FullName, id, message, severity, range);
        }
    }
}
