using System;
using System.Collections.Generic;
using System.Text;

namespace MusicConverterTest
{
    /// <summary>
    /// Provide interface and basic functions for Notes
    /// </summary>
    interface INote
    {
        /// <summary>
        /// Convert note to string for writing
        /// </summary>
        /// <param name="format">0 if maiData, 1 if GoodBrother</param>
        string Compose(int format);

        /// <summary>
        /// See if current note has all information needed
        /// </summary>
        /// <returns>True if qualified, false elsewise</returns>
        bool CheckValidity();
    }
}
