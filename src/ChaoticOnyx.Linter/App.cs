using System.Diagnostics.CodeAnalysis;
using System.IO;
using Spectre.Console.Cli;
using Console = Spectre.Console.AnsiConsole;

namespace DMLinter
{
    public sealed class AppRunSettings : CommandSettings
    {
        /// <summary>
        ///     Путь до файла среды .dme.
        /// </summary>
        [CommandArgument(0, "[path]")]
        public string DmePath { get; set; } = string.Empty;

        /// <summary>
        ///     Если true - анализаторы будут пытаться исправлять проблемы.
        /// </summary>
        [CommandOption("--fix")]
        public bool Fix { get; set; }

        /// <summary>
        ///     Если true - печатает отчёт после анализа.
        /// </summary>
        [CommandOption("--report")]
        public bool Report { get; set; }
    }

    public sealed class App : Command<AppRunSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] AppRunSettings settings)
        {
            FileInfo? dme;

            if (string.IsNullOrEmpty(settings.DmePath))
            {
                dme = Environment.FindDme();
            }
            else
            {
                dme = new FileInfo(settings.DmePath);
            }

            Console.MarkupLine(dme is null
                ? "[bold red].dme файл не найден.[/]"
                : $"Используется файл среды [bold green]{dme.FullName}[/]");

            if (dme is null)
            {
                return 1;
            }

            Environment env = new(dme);
            env.Load();
            env.Analyze(settings.Fix);

            if (settings.Report)
            {
                env.PrintReport();
            }

            env.Save();

            return 0;
        }
    }
}
