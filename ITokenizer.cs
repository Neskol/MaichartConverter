using System;
namespace MusicConverterTest
{
    /// <summary>
    /// Intake files and tokenize.
    /// </summary>
    partial interface ITokenizer
    {
        /// <summary>
        /// Intake files and return tokens.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        string[] Tokens(string location);
    }
}

