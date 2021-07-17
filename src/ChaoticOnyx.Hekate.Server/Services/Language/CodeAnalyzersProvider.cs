using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChaoticOnyx.Hekate.Server.Language;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public class CodeAnalyzersProvider : ICodeAnalyzersProvider
    {
        public List<CodeAnalyzer> GetAnalyzers()
        {
            var                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<CodeAnalyzer> analyzers  = new();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var analyzerTypes = (from t in assembly.GetExportedTypes() where typeof(CodeAnalyzer).IsAssignableFrom(t) && !t.IsAbstract select t).ToList();
                    analyzers.AddRange(analyzerTypes.Select(t => (CodeAnalyzer)Activator.CreateInstance(t)!));
                }
                catch (NotSupportedException) { }
            }

            return analyzers;
        }
    }
}
