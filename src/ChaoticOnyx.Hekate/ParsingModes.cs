using System;

namespace ChaoticOnyx.Hekate
{
    [Flags]
    public enum ParsingModes
    {
        /// <summary>
        ///     Используется только лексер.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Также используется препроцессор.
        /// </summary>
        Full = 1 << 1
    }
}
