using System;
using System.IO;
namespace MaichartConverter
{
    public class SimaiTokenizer : ITokenizer
    {
        TrackInformation trackInformation;
        public SimaiTokenizer()
        {
            trackInformation = new XmlInformation();
        }

        public string[] Tokens(string location)
        {

            string[] takeIn = File.ReadAllLines(location);
            string storage = "";
            foreach (string line in takeIn)
            {
                storage += line;
            }
            string[] result = storage.Split(",");
            return result;
        }

        public string[] TokensFromText(string text)
        {
            string storage = text;
            string[] result = storage.Split(",");
            return result;
        }
    }
}

