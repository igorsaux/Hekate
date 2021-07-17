using ChaoticOnyx.Hekate.Server.Services.Language;
using Xunit;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    public class CodeAnalyzersProviderTests
    {
        [Fact]
        public void ProvideTest()
        {
            // Arrange
            ICodeAnalyzersProvider analyzersProvider = new CodeAnalyzersProvider();

            // Act
            var analyzers = analyzersProvider.GetAnalyzers();

            // Assert
            Assert.NotEmpty(analyzers);
        }
    }
}
