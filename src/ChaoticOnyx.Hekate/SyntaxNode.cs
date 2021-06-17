namespace ChaoticOnyx.Hekate
{
    public class SyntaxNode
    {
        public SyntaxToken? Token { get; }

        public SyntaxNode(SyntaxToken? token) => Token = token;
    }
}
