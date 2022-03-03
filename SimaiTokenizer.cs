using System;
using System.IO;
namespace MaichartConverter
{
	public class SimaiTokenizer:ITokenizer
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
                    storage+=line;
                }
                string[] result = storage.Split('&');
                return result;
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(location);
            }
        }
    }
}

