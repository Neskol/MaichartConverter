using System;
using System.IO;
namespace MaichartConverter
{
    public class SimaiTokenizer : ITokenizer
    {
        public SimaiTokenizer()
        {
        }

        public string[] Tokens(string location)
        {
            try
            {
                string[] takeIn = File.ReadAllLines(location);
                string storage = "";
                foreach (string line in takeIn)
                {
                    storage += line;
                }
                string[] result = storage.Split('&');
                return result;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Exception raised: " + ex.Message);
                throw ex;
            }
        }

        public string[] TokensFromText(string text)
        {
            try
            {
                string storage = text;
                string[] result = storage.Split('&');
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

