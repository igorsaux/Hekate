using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChaoticOnyx.Hekate.Server.Services.FileProvider
{
    public interface IFileProvider<T>
    {
        /// <summary>
        ///     Считывает указанный файл и возвращает его содержимое.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Содержимое файла.</returns>
        public Task<ReadOnlyMemory<T>> ReadFileAsync(string fullPath, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Записывает в указанный файл новое содержимое.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="content">Содержимое для записи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача записи в файл.</returns>
        public ValueTask WriteFileAsync(string fullPath, ReadOnlyMemory<T> content, CancellationToken cancellationToken = default);
    }
}
