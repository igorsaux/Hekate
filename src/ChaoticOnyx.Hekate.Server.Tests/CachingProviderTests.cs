using System;
using System.IO;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Services.CacheProvider;
using ChaoticOnyx.Hekate.Server.Services.FileProvider;
using Xunit;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    [Collection("IO")]
    public class CachingProviderTests : IDisposable
    {
        private CachedFileProvider _provider = new(new TextFileProvider());

        [Fact]
        public async Task ReadingFileTestAsync()
        {
            // Arrange
            const string text = "Some test string";
            var          file = new FileInfo("test.txt");
            TestUtils.WriteFile(file, text);

            // Act
            var content = await _provider.Resolve(file.FullName);

            // Assert
            Assert.Equal(text, content.ToString());
        }

        [Fact]
        public async Task WritingFileTestAsync()
        {
            // Arrange
            const string text = "Some test string";
            var          file = new FileInfo("test.txt");

            // Act
            await _provider.Update(file.FullName, text.AsMemory());
            string result = TestUtils.ReadFile(file);

            // Assert
            Assert.Equal(text, result);
        }

        [Fact]
        public async Task RefreshingCacheTestAsync()
        {
            // Arrange
            const string text1 = "Some test string";
            const string text2 = "Not previous string";
            var          file  = new FileInfo("test.txt");
            TestUtils.WriteFile(file, text1);

            // Act
            var result1 = await _provider.Resolve(file.FullName);
            TestUtils.WriteFile(file, text2);
            var result2 = await _provider.Resolve(file.FullName);

            // Assert
            Assert.Equal(text1, result1.ToString());
            Assert.Equal(text2, result2.ToString());
        }

        public void Dispose()
        {
            TestUtils.ClearCreatedFiles();
            GC.SuppressFinalize(this);
        }
    }
}
