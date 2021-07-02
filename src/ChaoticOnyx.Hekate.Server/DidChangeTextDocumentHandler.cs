using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Serilog;

namespace ChaoticOnyx.Hekate.Server
{
    public class DidChangeTextDocumentHandler : IDidChangeTextDocumentHandler
    {
        private readonly DocumentSelector _selector = new(new DocumentFilter
        {
            Pattern = "**/*.dm"
        });

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
        {
            Log.Logger.Debug($"Some change in {request.TextDocument.Uri}");

            return Unit.Task;
        }

        public TextDocumentChangeRegistrationOptions GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
            => new()
            {
                DocumentSelector = _selector
            };
    }
}
