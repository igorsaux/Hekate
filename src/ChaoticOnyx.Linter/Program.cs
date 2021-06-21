using Spectre.Console.Cli;

namespace ChaoticOnyx.Linter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandApp<App>();

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
