using Spectre.Console.Cli;

namespace ChaoticOnyx.Hekate.Linter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommandApp<App> app = new();

            app.Configure(configuration =>
            {
#if DEBUG
                configuration.PropagateExceptions();
                configuration.ValidateExamples();
#endif
            });

            app.Run(args);
        }
    }
}
