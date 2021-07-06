using System.Collections.Generic;
using System.IO;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    internal static class TestUtils
    {
        private static List<FileInfo> s_createdFiles = new();

        public static void WriteFile(FileInfo file, string content)
        {
            using var writer = File.CreateText(file.FullName);
            writer.Write(content);
            s_createdFiles.Add(file);
        }

        public static string ReadFile(FileInfo file)
        {
            using var reader = File.OpenText(file.FullName);

            return reader.ReadToEnd();
        }

        public static void ClearCreatedFiles()
        {
            foreach (var file in s_createdFiles)
            {
                file.Delete();
            }
        }
    }
}
