#nullable disable

using System.Text.Json.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server
{
    public sealed class Settings
    {
        public DiagnosticSettings[] DiagnosticSettings { get; init; }
    }

    public sealed class DiagnosticSettings
    {
        public string Id      { get; init; }
        public bool   Enabled { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DiagnosticSeverity Severity { get; init; }
    }
}
