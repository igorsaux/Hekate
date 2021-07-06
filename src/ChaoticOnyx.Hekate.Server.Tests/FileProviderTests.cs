using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Services.CacheProvider;
using ChaoticOnyx.Hekate.Server.Services.FileProvider;
using Xunit;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    [Collection("IO")]
    public class CachingTest : IDisposable
    {
        private TextFileProvider _provider    = new();
        private List<FileInfo>   _createFiles = new();

        [Fact]
        public async Task ReadingFileTestAsync()
        {
            // Arrange
            var    file     = new FileInfo("test.txt");
            string text     = "Some test string";
            TestUtils.WriteFile(file, text);

            // Act
            var result = await _provider.ReadFileAsync(file.FullName, CancellationToken.None);

            // Assert
            Assert.Equal(text, result.ToString());
        }

        [Fact]
        public async Task WritingFileTestAsync()
        {
            // Arrange
            var    file = new FileInfo("test.txt");
            string text = "Some test string";

            // Act
            await _provider.WriteFileAsync(file.FullName, text.AsMemory());
            string result = TestUtils.ReadFile(file);
            file.Delete();

            // Assert
            Assert.Equal(text, result);
        }

        public void Dispose()
        {
            TestUtils.ClearCreatedFiles();
            GC.SuppressFinalize(this);
        }
    }
}
