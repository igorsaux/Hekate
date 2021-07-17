using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Extensions;
using ChaoticOnyx.Hekate.Server.Language;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public sealed class DmEnvironmentService : IDmEnvironmentService
    {
        public           List<CodeFile>          Files { get; } = new();
        private readonly IDmLanguageService      _languageService;
        private readonly ICodeAnalyzersProvider? _codeAnalyzersProvider;
        private readonly ILanguageServerFacade?  _serverFacade;

        public DmEnvironmentService(IDmLanguageService languageService, ICodeAnalyzersProvider codeAnalyzersProvider, ILanguageServerFacade serverFacade)
        {
            _languageService            = languageService;
            _codeAnalyzersProvider = codeAnalyzersProvider;
            _serverFacade               = serverFacade;
        }

        public DmEnvironmentService(IDmLanguageService languageService)
        {
            _languageService            = languageService;
        }

        public async Task ParseEnvironmentAsync(FileInfo dme, CancellationToken cancellationToken = default)
        {
            Files.Clear();
            await ParseRecursiveAsync(dme, new PreprocessorContext(), cancellationToken);
            CollectDiagnosticsAndPublish();
        }

        private async Task ParseRecursiveAsync(FileInfo file, PreprocessorContext context, CancellationToken cancellationToken = default)
        {
            var parsedFile = await _languageService.ParseAsync(file, context, cancellationToken);
            Files.Add(parsedFile);

            foreach (var includeDirective in parsedFile.PreprocessorContext.Includes)
            {
                string includePath = includeDirective.Text[1..^1];
                includePath = Path.GetFullPath(includePath, file.DirectoryName!);
                await ParseRecursiveAsync(new FileInfo(includePath), parsedFile.PreprocessorContext, cancellationToken);
            }
        }

        private void CollectDiagnosticsAndPublish()
        {
            if (_serverFacade is null)
            {
                return;
            }

            var diagnostics = DoCodeAnalysis();

            if (diagnostics is null)
            {
                _serverFacade.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
                {
                    Diagnostics = new Container<Diagnostic>()
                });

                return;
            }

            foreach (var kvp in diagnostics)
            {
                PublishDiagnostics(kvp.Key, kvp.Value);
            }
        }

        private void PublishDiagnostics(string filePath, Container<Diagnostic> diagnostics)
        {
            if (_serverFacade is null)
            {
                return;
            }

            foreach (var kvp in diagnostics)
            {
                _serverFacade.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
                {
                    Uri         = DocumentUri.File(filePath),
                    Diagnostics = diagnostics
                });
            }
        }

        private Dictionary<string, List<Diagnostic>>? DoCodeAnalysis()
        {
            if (_codeAnalyzersProvider is null)
            {
                return null;
            }

            var analyzers = _codeAnalyzersProvider.GetAnalyzers();

            foreach (var file in Files)
            {
                for (LinkedListNode<SyntaxToken>? token = file.Tokens.First; token != null; token = token.Next)
                {
                    VisitContext context = new(file, token, file.PreprocessorContext);

                    foreach (CodeAnalyzer analyzer in analyzers)
                    {
                        analyzer.OnVisit(context);
                    }
                }
            }

            Dictionary<string, List<Diagnostic>> result = new();

            foreach (CodeAnalyzer analyzer in analyzers)
            {
                var diagnostics = analyzer.Diagnostics;

                foreach (var kvp in diagnostics)
                {
                    if (result.ContainsKey(kvp.Key))
                    {
                        result[kvp.Key]
                            .AddRange(kvp.Value);
                    }
                    else
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return result;
        }
    }
}
