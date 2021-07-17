using System.Collections.Generic;
using ChaoticOnyx.Hekate.Server.Language;

namespace ChaoticOnyx.Hekate.Server.Services.Language
{
    public interface ICodeAnalyzersProvider
    {
        /// <summary>
        ///     Возвращает список новых инициализированных анализаторов.
        /// </summary>
        /// <returns></returns>
        public List<CodeAnalyzer> GetAnalyzers();
    }
}
