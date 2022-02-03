namespace MaichartConverter
{
    public class Ma2Tokenizer : ITokenizer
    {
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
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(location);
            }
        }
    }
}

