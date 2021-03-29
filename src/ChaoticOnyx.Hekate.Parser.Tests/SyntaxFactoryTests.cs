#region

using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
    public class SyntaxFactoryTests
    {
        [Fact]
        public void SingleLineCommentTest()
        {
            // Arrange
            var expected = "// This is a comment\n";
            var token    = SyntaxFactory.SingleLineComment(" This is a comment");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultiLineCommentTest()
        {
            // Arrange
            var expected = "/*\n  Hello!\n*/";
            var token    = SyntaxFactory.MultiLineComment("\n  Hello!\n");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IdentifierTest()
        {
            // Arrange
            var expected = "var";
            var token    = SyntaxFactory.Identifier("var");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TextLiteralTest()
        {
            // Arrange
            var expected = "\"Test\"";
            var token    = SyntaxFactory.TextLiteral("Test");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NumericalLiteralTest()
        {
            // Arrange
            var expected = "12 7.8 12.9";

            SyntaxToken[] tokens =
            {
                SyntaxFactory.NumericalLiteral(12)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral(7.8)
                             .WithTrails(SyntaxFactory.WhiteSpace(" ")),
                SyntaxFactory.NumericalLiteral((float) 12.9)
            };

            // Act
            var unit   = new CompilationUnit(tokens);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PathLiteralTest()
        {
            // Arrange
            var expected = "'sound/mysound.ogg'";
            var token    = SyntaxFactory.PathLiteral("sound/mysound.ogg");

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ForKeywordTest()
        {
            // Arrange
            var expected = "for";
            var token    = SyntaxFactory.ForKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NewKeywordTest()
        {
            // Arrange
            var expected = "new";
            var token    = SyntaxFactory.NewKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GlobalKeywordTest()
        {
            // Arrange
            var expected = "global";
            var token    = SyntaxFactory.GlobalKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ThrowKeywordTest()
        {
            // Arrange
            var expected = "throw";
            var token    = SyntaxFactory.ThrowKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CatchKeywordTest()
        {
            // Arrange
            var expected = "catch";
            var token    = SyntaxFactory.CatchKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryKeywordTest()
        {
            // Arrange
            var expected = "try";
            var token    = SyntaxFactory.TryKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VarKeywordTest()
        {
            // Arrange
            var expected = "var";
            var token    = SyntaxFactory.VarKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerbKeywordTest()
        {
            // Arrange
            var expected = "verb";
            var token    = SyntaxFactory.VerbKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProcKeywordTest()
        {
            // Arrange
            var expected = "proc";
            var token    = SyntaxFactory.ProcKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InKeywordTest()
        {
            // Arrange
            var expected = "in";
            var token    = SyntaxFactory.InKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfKeywordTest()
        {
            // Arrange
            var expected = "if";
            var token    = SyntaxFactory.IfKeyword();

            // Act
            var unit  = new CompilationUnit(token);
            var resul = unit.Emit();

            // Assert
            Assert.Equal(expected, resul);
        }

        [Fact]
        public void ElseKeywordTest()
        {
            // Arrange
            var expected = "else";
            var token    = SyntaxFactory.ElseKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetKeywordTest()
        {
            // Arrange
            var expected = "set";
            var token    = SyntaxFactory.SetKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsKeywordTest()
        {
            // Arrange
            var expected = "as";
            var token    = SyntaxFactory.AsKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhileKeywordTest()
        {
            // Arrange
            var expected = "while";
            var token    = SyntaxFactory.WhileKeyword();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DefineDirectiveTest()
        {
            // Arrange
            var expected = "#define\n";
            var token    = SyntaxFactory.DefineDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IncludeDirectiveTest()
        {
            // Arrange
            var expected = "#include\n";
            var token    = SyntaxFactory.IncludeDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfDefDirective()
        {
            // Arrange
            var expected = "#ifdef\n";
            var token    = SyntaxFactory.IfDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfNDefDirective()
        {
            // Arrange
            var expected = "#ifndef\n";
            var token    = SyntaxFactory.IfNDefDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EndIfDirective()
        {
            // Arrange
            var expected = "#endif\n";
            var token    = SyntaxFactory.EndIfDirective();

            // Act
            var unit   = new CompilationUnit(token);
            var result = unit.Emit();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
