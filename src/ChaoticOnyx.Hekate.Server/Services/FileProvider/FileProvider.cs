using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.FileProvider
{
    /// <summary>
    ///     Считывает файлы в байтовое представление.
    /// </summary>
    public class FileProvider : IFileProvider<byte>
    {
        public async Task<ReadOnlyMemory<byte>> ReadFileAsync(string fullPath, CancellationToken cancellationToken = default)
        {
            await using var reader  = File.OpenRead(fullPath);
            Memory<byte>    content = new(new byte[reader.Length]);
            await reader.ReadAsync(content, cancellationToken);

            return content;
        }

        public async ValueTask WriteFileAsync(string fullPath, ReadOnlyMemory<byte> content, CancellationToken cancellationToken = default)
        {
            await using var writer = File.OpenWrite(fullPath);
            await writer.WriteAsync(content, cancellationToken);
        }
    }
}
