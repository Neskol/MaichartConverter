using System;
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
                string[] takein = System.IO.File.ReadAllLines(location);
                List<char> storage = new List<char>();
                foreach (string line in takein)
                {
                    storage.AddRange(line.ToCharArray());
                }
                string[] result = new string[storage.Count];
                for (int i=0;i<storage.Count;i++)
                {
                    result[i] = storage[i].ToString();
                }
                return result;
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException(location);
            }
        }
    }
}

