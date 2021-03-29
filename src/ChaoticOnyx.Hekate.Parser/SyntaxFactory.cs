#region

using System.Globalization;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     API для создания токенов.
    /// </summary>
    public static class SyntaxFactory
    {
        public static SyntaxToken WhiteSpace(string space)
        {
            return new(SyntaxKind.WhiteSpace, space);
        }

        public static SyntaxToken EndOfLine(string ending = "\n")
        {
            return new(SyntaxKind.EndOfLine, ending);
        }

        public static SyntaxToken EndOfFile()
        {
            return new(SyntaxKind.EndOfFile, "");
        }

        public static SyntaxToken SingleLineComment(string text, string ending = "\n")
        {
            text = $"//{text}";

            return new SyntaxToken(SyntaxKind.SingleLineComment, text).WithTrails(EndOfLine(ending));
        }

        public static SyntaxToken MultiLineComment(string text)
        {
            text = $"/*{text}*/";

            return new(SyntaxKind.MultiLineComment, text);
        }

        public static SyntaxToken Identifier(string name)
        {
            return new(SyntaxKind.Identifier, name);
        }

        public static SyntaxToken TextLiteral(string text)
        {
            return new(SyntaxKind.TextLiteral, $"\"{text}\"");
        }

        public static SyntaxToken NumericalLiteral(int number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString());
        }

        public static SyntaxToken NumericalLiteral(float number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));
        }

        public static SyntaxToken NumericalLiteral(double number)
        {
            return new(SyntaxKind.NumericalLiteral, number.ToString(CultureInfo.InvariantCulture));
        }

        public static SyntaxToken PathLiteral(string path)
        {
            return new(SyntaxKind.PathLiteral, $"'{path}'");
        }

        public static SyntaxToken ForKeyword()
        {
            return new(SyntaxKind.ForKeyword, "for");
        }

        public static SyntaxToken NewKeyword()
        {
            return new(SyntaxKind.NewKeyword, "new");
        }

        public static SyntaxToken GlobalKeyword()
        {
            return new(SyntaxKind.GlobalKeyword, "global");
        }

        public static SyntaxToken ThrowKeyword()
        {
            return new(SyntaxKind.ThrowKeyword, "throw");
        }

        public static SyntaxToken CatchKeyword()
        {
            return new(SyntaxKind.CatchKeyword, "catch");
        }

        public static SyntaxToken TryKeyword()
        {
            return new(SyntaxKind.TryKeyword, "try");
        }

        public static SyntaxToken VarKeyword()
        {
            return new(SyntaxKind.VarKeyword, "var");
        }

        public static SyntaxToken VerbKeyword()
        {
            return new(SyntaxKind.VerbKeyword, "verb");
        }

        public static SyntaxToken ProcKeyword()
        {
            return new(SyntaxKind.ProcKeyword, "proc");
        }

        public static SyntaxToken InKeyword()
        {
            return new(SyntaxKind.InKeyword, "in");
        }

        public static SyntaxToken IfKeyword()
        {
            return new(SyntaxKind.IfKeyword, "if");
        }

        public static SyntaxToken ElseKeyword()
        {
            return new(SyntaxKind.ElseKeyword, "else");
        }

        public static SyntaxToken SetKeyword()
        {
            return new(SyntaxKind.SetKeyword, "set");
        }

        public static SyntaxToken AsKeyword()
        {
            return new(SyntaxKind.AsKeyword, "as");
        }

        public static SyntaxToken WhileKeyword()
        {
            return new(SyntaxKind.WhileKeyword, "while");
        }

        public static SyntaxToken DefineDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.DefineDirective, "#define").WithTrails(EndOfLine(ending));
        }

        public static SyntaxToken IncludeDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IncludeDirective, "#include").WithTrails(EndOfLine(ending));
        }

        public static SyntaxToken IfDefDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IfDefDirective, "#ifdef").WithTrails(EndOfLine(ending));
        }

        public static SyntaxToken IfNDefDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.IfNDefDirective, "#ifndef").WithTrails(EndOfLine(ending));
        }

        public static SyntaxToken EndIfDirective(string ending = "\n")
        {
            return new SyntaxToken(SyntaxKind.EndIfDirective, "#endif").WithTrails(EndOfLine(ending));
        }
    }

    public static class SyntaxTokenExtensions
    {
        public static SyntaxToken WithLeads(this SyntaxToken token, params SyntaxToken[] leads)
        {
            token.AddLeadTokens(leads);

            return token;
        }

        public static SyntaxToken WithTrails(this SyntaxToken token, params SyntaxToken[] trails)
        {
            token.AddTrailTokens(trails);

            return token;
        }
    }
}
