using System;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Server;

namespace ChaoticOnyx.Hekate.Server
{
    internal class Program
    {
        private static void ConfigureServer(LanguageServerOptions options)
            => options.WithInput(Console.OpenStandardInput())
                      .WithOutput(Console.OpenStandardOutput());

        private static async Task Main(string[] args)
        {
            LanguageServer server = await LanguageServer.From(ConfigureServer);
            await server.WaitForExit;
        }
    }
}
