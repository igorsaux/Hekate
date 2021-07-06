using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.FileProvider
{
    /// <summary>
    ///     Считывает файлы в текстовом представлении.
    /// </summary>
    public sealed class TextFileProvider : FileProvider, IFileProvider<char>
    {
        public new async Task<ReadOnlyMemory<char>> ReadFileAsync(string fullPath, CancellationToken cancellationToken = default)
        {
            var content = await base.ReadFileAsync(fullPath, cancellationToken);

            return new ReadOnlyMemory<char>(Encoding.UTF8.GetString(content.Span)
                                                    .ToCharArray());
        }

        public async ValueTask WriteFileAsync(string fullPath, ReadOnlyMemory<char> content, CancellationToken cancellationToken = default)
        {
            int bufferSize = Encoding.UTF8.GetByteCount(content.Span);
            Memory<byte> memory = new(new byte[bufferSize]);
            Encoding.UTF8.GetBytes(content.Span, memory.Span);
            await base.WriteFileAsync(fullPath, memory, cancellationToken);
        }
    }
}
