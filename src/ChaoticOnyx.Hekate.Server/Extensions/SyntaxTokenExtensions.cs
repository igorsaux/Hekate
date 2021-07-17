using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server.Extensions
{
    public static class SyntaxTokenExtensions
    {
        public static Range GetRange(this SyntaxToken token)
        {
            FileLine position      = token.FilePosition;

            return new Range(position.Line, position.Column, position.Line, position.Column + token.Length);
        }
    }
}
