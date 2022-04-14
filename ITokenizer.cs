namespace MaichartConverter
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
        /// <returns>Tokens from file specified</returns>
        string[] Tokens(string location);

        /// <summary>
        /// Intake files and return tokens.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Tokens from text specified</returns>
        string[] TokensFromText(string text);
    }
}

