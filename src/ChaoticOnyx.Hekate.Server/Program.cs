using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;

namespace ChaoticOnyx.Hekate.Server
{
    internal class Program
    {
        private static FileInfo FindDme(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            FileInfo?  dme   = files.FirstOrDefault(file => file.Extension == ".dme");

            if (dme is null)
            {
                throw new FileNotFoundException(".dme файл не найден.");
            }

            return dme;
        }

        private static void ConfigureServer(LanguageServerOptions options)
            => options.WithInput(Console.OpenStandardInput())
                      .WithOutput(Console.OpenStandardOutput())
                      .WithServices(services => services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Trace)));

        private static async Task Main()
        {
            LanguageServer server = await LanguageServer.From(ConfigureServer);
            await server.WaitForExit;
        }
    }
}
