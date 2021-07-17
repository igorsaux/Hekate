using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public record VisitContext(CodeFile CodeFile, LinkedListNode<SyntaxToken> Token, PreprocessorContext PreprocessorContext);
}
