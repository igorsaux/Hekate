using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Services.FileProvider;

namespace ChaoticOnyx.Hekate.Server.Services.CacheProvider
{
    public class CachedFileProvider : ICacheProvider<string, ReadOnlyMemory<char>>
    {
        /// <summary>
        ///     Словарь который соотносит абсолютный путь файла к его содержимому.
        /// </summary>
        private readonly Dictionary<string, CachedFile> _filesCache = new();

        private sealed class CachedFile
        {
            public ReadOnlyMemory<char> Content   { get; set; }
            public DateTime             LastWrite { get; set; }

            public CachedFile(ReadOnlyMemory<char> content, DateTime lastWrite)
            {
                Content   = content;
                LastWrite = lastWrite;
            }
        }

        private readonly IFileProvider<char> _fileProvider;

        public CachedFileProvider(IFileProvider<char> fileProvider)
        {
            _fileProvider = fileProvider;
        }

        /// <summary>
        ///     Возвращает содержимое файла по указанному пути.
        /// </summary>
        /// <param name="key">Полный путь до файла.</param>
        /// <returns>Содержимое файла.</returns>
        public async Task<ReadOnlyMemory<char>> Resolve(string key)
        {
            CachedFile? cached;

            if (_filesCache.ContainsKey(key))
            {
                cached        = _filesCache[key];
                DateTime lastWriteTime = File.GetLastWriteTime(key);

                if (cached.LastWrite == lastWriteTime)
                {
                    return cached.Content;
                }

                cached.LastWrite = lastWriteTime;
                cached.Content   = await _fileProvider.ReadFileAsync(key);

                return cached.Content;
            }

            cached = new CachedFile(await _fileProvider.ReadFileAsync(key), File.GetLastWriteTime(key));
            _filesCache.Add(key, cached);

            return cached.Content;
        }

        public async Task Update(string key, ReadOnlyMemory<char> value)
        {
            await _fileProvider.WriteFileAsync(key, value);

            if (_filesCache.ContainsKey(key))
            {
                _filesCache[key]
                    .Content = value;

                _filesCache[key]
                    .LastWrite = File.GetLastWriteTime(key);
            }
            else
            {
                _filesCache.Add(key, new CachedFile(value, File.GetLastWriteTime(key)));
            }
        }
    }
}
