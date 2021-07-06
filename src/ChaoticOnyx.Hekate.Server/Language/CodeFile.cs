using System;
using System.Collections.Generic;
using System.IO;
using ChaoticOnyx.Hekate.Scaffolds;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public sealed class CodeFile
    {
        private Memory<char>            _code = Memory<char>.Empty;
        public  FileInfo                File                { get; set; }
        public  List<CodeIssue>         Issues              { get; set; } = new();
        public  LinkedList<SyntaxToken> Tokens              { get; set; } = new();
        public  PreprocessorContext     PreprocessorContext { get; set; } = new();

        public CodeFile(FileInfo file) => File = file;

        public void Read()
            => _code = new Memory<char>(System.IO.File.ReadAllText(File.FullName)
                                              .ToCharArray());

        public void Parse(Lexer lexer)
        {
            TextToTokensScaffold? scaffold = new(_code, lexer);
            Tokens = scaffold.GetResult();
        }

        public void Preprocess(Preprocessor preprocessor, PreprocessorContext context) => PreprocessorContext = preprocessor.Preprocess(Tokens, context);
    }
}
