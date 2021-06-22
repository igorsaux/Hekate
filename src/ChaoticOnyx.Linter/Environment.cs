using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ChaoticOnyx.Hekate;
using ChaoticOnyx.Linter.Analyzers;
using Spectre.Console;
using Console = Spectre.Console.AnsiConsole;

namespace ChaoticOnyx.Linter
{
    public sealed class Environment
    {
        public static List<CodeAnalyzer> CodeAnalyzers = new()
        {
            new SpaceAnalyzer(),
            new SpansAnalyzer()
        };

        private Dictionary<FileInfo, AnalysisResult>  _analysisResults { get; } = new Dictionary<FileInfo, AnalysisResult>();
        private Dictionary<FileInfo, CompilationUnit> _files           { get; } = new Dictionary<FileInfo, CompilationUnit>();

        /// <summary>
        ///     Все включённые файлы в проекте.
        /// </summary>
        public IImmutableDictionary<FileInfo, CompilationUnit> Files => _files.ToImmutableDictionary();

        /// <summary>
        ///     Результаты анализа.
        /// </summary>
        public IImmutableDictionary<FileInfo, AnalysisResult> AnalysisResults => _analysisResults.ToImmutableDictionary();

        /// <summary>
        ///     Файл среды.
        /// </summary>
        public FileInfo Dme { get; }

        public Environment(FileInfo dmeFile) => Dme = dmeFile;

        /// <summary>
        ///     Находит .dme файл в текущей рабочей директории.
        /// </summary>
        /// <returns>Путь до .dme файла.</returns>
        public static FileInfo? FindDme()
        {
            string[] files   = Directory.GetFiles(Directory.GetCurrentDirectory());
            string?  dmePath = files.FirstOrDefault(fileName => Path.GetExtension(fileName) == ".dme");

            return string.IsNullOrEmpty(dmePath)
                ? null
                : new FileInfo(dmePath);
        }

        /// <summary>
        ///     Парсит файл серды и все включённые файлы рекурсивно.
        /// </summary>
        public void Load()
        {
            Console.Status()
                   .Spinner(Spinner.Known.Star)
                   .Start("Парсинг файлов...", ctx =>
                   {
                       PreprocessorContext preprocessorContext = PreprocessorContext.Empty;
                       ParseFile(Dme, ref preprocessorContext);
                   });

            Console.MarkupLine($"Обнаружено файлов: [bold yellow]{_files.Count}[/].");
        }

        /// <summary>
        ///     Производит анализ файлов.
        /// </summary>
        /// <param name="tryToFix">Если true - анализаторы будут пытаться исправлять ошибки.</param>
        public void Analyze(bool tryToFix)
        {
            int issuesCount = 0;

            Console.MarkupLine(tryToFix
                ? "Авто исправления: [bold green]включены[/]."
                : "Авто исправления: [bold red]выключены[/].");

            Console.MarkupLine($"Используется анализаторов: [bold green]{CodeAnalyzers.Count}[/].");

            Console.Status()
                   .Spinner(Spinner.Known.Star)
                   .Start("Анализ файлов...", ctx =>
                   {
                       foreach (var (file, unit) in _files)
                       {
                           var result = Analyze(tryToFix, file);
                           issuesCount += result.CodeIssues.Count;
                           issuesCount += unit.GetIssues().Count;
                           _analysisResults.Add(file, result);
                       }
                   });

            Console.MarkupLine(issuesCount > 0
                ? $"Найдено ошибок: [bold red]{issuesCount}[/]."
                : "Ошибок [bold green]не найдено[/].");
        }

        /// <summary>
        ///     Производит анализ файла.
        /// </summary>
        /// <param name="tryToFix">Если true - анализаторы будут пытаться исправлять ошибки.</param>
        /// <param name="file">Анализировать только определённый файл.</param>
        public AnalysisResult Analyze(bool tryToFix, FileInfo file)
        {
            var             issues = new List<CodeIssue>();
            CompilationUnit? lastFixedUnit = null;
            var             unit    = _files[file];
            AnalysisContext context = new(CodeStyle.Default, unit, tryToFix);

            foreach (var analyzer in CodeAnalyzers)
            {
                AnalysisResult result = analyzer.Call(context);

                if (result.CodeIssues.Count == 0 && result.FixedUnit == null)
                {
                    continue;
                }

                if (tryToFix && result.FixedUnit is not null)
                {
                    context = context with
                    {
                        Unit = result.FixedUnit
                    };

                    lastFixedUnit = result.FixedUnit;
                }

                issues.AddRange(result.CodeIssues);
            }

            return new AnalysisResult(issues.ToImmutableList(), lastFixedUnit);
        }

        /// <summary>
        ///     Сохраняет все изменённые файлы.
        /// </summary>
        public void Save()
        {
            foreach (var (file, result) in _analysisResults)
            {
                if (result.FixedUnit is null)
                {
                    continue;
                }

                Console.MarkupLine($"Сохранение [bold yellow]{file.FullName}[/]");
                File.WriteAllText(file.FullName, result.FixedUnit.Emit());
            }
        }

        /// <summary>
        ///     Печатает отчёт о найденных ошибках.
        /// </summary>
        public void PrintReport()
        {
            foreach (var (file, result) in _analysisResults)
            {
                if (result.CodeIssues.Count == 0)
                {
                    continue;
                }

                var relativePath = Path.GetRelativePath(Dme.DirectoryName!, file.FullName);

                var table = new Table
                {
                    Border = TableBorder.Rounded
                };

                table.LeftAligned();
                table.AddColumns("", $"[link={Uri.EscapeUriString(file.FullName)}]{relativePath}[/]");

                foreach (var issue in result.CodeIssues)
                {
                    var position = issue.Token.FilePosition;
                    var message  = string.Format(Issues.IdToMessage(issue.Id), issue.Arguments);
                    table.AddRow($"[bold red]{issue.Id}[/]", $"[bold yellow]{relativePath}:{position.Line}:{position.Column} {message}[/]");
                }

                Console.Render(table);
            }
        }

        /// <summary>
        ///     Рекурсивно парсит файлы начиная с файла среды.
        /// </summary>
        /// <param name="file">Файл для парсинга.</param>
        /// <param name="context">Контекст препроцессора.</param>
        private void ParseFile(FileInfo file, ref PreprocessorContext context)
        {
            if (_files.ContainsKey(file))
            {
                return;
            }

            CompilationUnit unit    = CompilationUnit.FromSource(File.ReadAllText(file.FullName));
            unit.Parse(context);
            _files.Add(file, unit);

            context = context with
            {
                Defines = unit.Context.Defines
            };

            foreach (var include in unit.Context.Includes)
            {
                string includePath = include.Text[1..^1];
                includePath = Path.GetFullPath(includePath, file.DirectoryName!);
                FileInfo includeFile = new FileInfo(includePath);

                if (includeFile.Extension is not ".dme" and not ".dm")
                {
                    continue;
                }

                ParseFile(includeFile, ref context);
            }
        }
    }
}
