#region

using System.Collections.ObjectModel;
using Xunit;

#endregion

namespace ChaoticOnyx.Hekate.Parser.Tests
{
	public class LexerIssuesTests
	{
		[Fact]
		public void Dm0001ErrorSingleQuote()
		{
			// Arrange
			CompilationUnit unit = new("\'");

			// Act
			unit.Parse();
			ReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

			// Assert
			Assert.True(errors.Count == 1);

			Assert.True(errors[0]
							.Id == IssuesId.MissingClosingSign);
		}

		[Fact]
		public void Dm0001ErrorDoubleQuote()
		{
			// Arrange
			CompilationUnit unit = new("\"");

			// Act
			unit.Parse();
			ReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

			// Assert
			Assert.True(errors.Count == 1);

			Assert.True(errors[0]
							.Id == IssuesId.MissingClosingSign);
		}

		[Fact]
		public void Dm0001ErrorMultiLineComment()
		{
			// Arrange
			CompilationUnit unit = new("/* Comment without end *");

			// Act
			unit.Parse();
			ReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

			// Assert
			Assert.True(errors.Count == 1);

			Assert.True(errors[0]
							.Id == IssuesId.MissingClosingSign);
		}

		[Fact]
		public void Dm0002Error()
		{
			// Arrange
			CompilationUnit unit = new("$token");

			// Act
			unit.Parse();
			ReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

			// Assert
			Assert.True(errors.Count == 1);

			Assert.True(errors[0]
							.Id == IssuesId.UnexpectedToken);
		}

		[Fact]
		public void Dm0003Error()
		{
			// Arrange
			CompilationUnit unit = new("#pragma");

			// Act
			unit.Parse();
			ReadOnlyCollection<CodeIssue> errors = unit.Lexer.Issues;

			// Assert
			Assert.True(errors.Count == 1);

			Assert.True(errors[0]
							.Id == IssuesId.UnknownDirective);
		}
	}
}
