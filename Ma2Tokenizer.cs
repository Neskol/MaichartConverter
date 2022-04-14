namespace MaichartConverter
{
    /// <summary>
    /// Tokenizer of ma2 file
    /// </summary>
    public class Ma2Tokenizer : ITokenizer
    {
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Ma2Tokenizer()
        {
        }

        public string[] Tokens(string location)
        {
            try
            {
                string[] result = System.IO.File.ReadAllLines(location);
                return result;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Exception raised: "+ex.Message);
                throw ex;
            }
        }

        public string[] TokensFromText(string text)
        {
            try
            {
                string[] result = text.Split("\n");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised: " + ex.Message);
                throw ex;
            }
        }
    }
}

