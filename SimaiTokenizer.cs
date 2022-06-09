using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MaichartConverter
{
    /// <summary>
    /// Tokenize input file into tokens that parser can read
    /// </summary>
    public class SimaiTokenizer : ITokenizer
    {
        /// <summary>
        /// Stores the candidates of charts
        /// </summary>
        private Dictionary<string,string[]> chartCandidates;

        /// <summary>
        /// Stores the information to read
        /// </summary>
        private TrackInformation simaiTrackInformation;

        /// <summary>
        /// Constructs a tokenizer
        /// </summary>
        public SimaiTokenizer()
        {
            this.simaiTrackInformation = new XmlInformation();
            this.chartCandidates = new Dictionary<string,string[]>();
        }

        /// <summary>
        /// Access the chart candidates
        /// </summary>
        public Dictionary<string, string[]> ChartCandidates
        {
            get { return this.chartCandidates; }
        }


        /// <summary>
        /// Update candidates from texts specified
        /// </summary>
        /// <param name="input">Text to be tokenized</param>
        public void UpdateFromText(string input)
        {
            string storage = input;
            string[] result = storage.Split("&");
            string titleCandidate = "";
            string bpmCandidate = "";
            string artistCandidate = "";
            string chartDesigner = "";
            string shortIdCandidate = "";
            string genreCandidate = "";
            string versionCandidate = "";

            foreach (string item in result)
            {
                if (item.Contains("title"))
                {
                    titleCandidate = item.Replace("title=","").Replace("[SD]","").Replace("[DX]","");
                    this.simaiTrackInformation.Information["Name"] = titleCandidate;
                }
                else if(item.Contains("wholebpm"))
                {
                    bpmCandidate = item.Replace("wholebpm=", "");
                    this.simaiTrackInformation.Information["BPM"] = bpmCandidate;
                }
                else if (item.Contains("artist"))
                {
                    artistCandidate = item.Replace("artist=", "");
                    this.simaiTrackInformation.Information["Composer"] = artistCandidate;
                }
                else if (item.Contains("des="))
                {
                    chartDesigner = item.Replace("des=", "");
                }
                else if (item.Contains("shortid"))
                {
                    shortIdCandidate = item.Replace("shortid=", "");
                    this.simaiTrackInformation.Information["Music ID"] = shortIdCandidate;
                    if (shortIdCandidate.Length<=6 && int.TryParse(shortIdCandidate,out int id))
                    {
                        if (shortIdCandidate.Length>4)
                        {
                            this.simaiTrackInformation.Information["SDDX Suffix"] = "DX";
                        }
                        else this.simaiTrackInformation.Information["SDDX Suffix"] = "SD";
                    }
                }
                else if (item.Contains("genre"))
                {
                    genreCandidate = item.Replace("genre=", "");
                    this.simaiTrackInformation.Information["Genre"] = genreCandidate;
                }
                else if (item.Contains("version"))
                {
                    versionCandidate = item.Replace("version=", "");
                    this.simaiTrackInformation.Information["Version"] = versionCandidate;
                }
                else if (item.Contains("lv_1"))
                {
                    string easyCandidate = item.Replace("lv_1=", "");
                    this.simaiTrackInformation.Information["Easy"] = easyCandidate;
                }
                else if (item.Contains("des_1"))
                {
                    string easyChartCandidate = item.Replace("des_1=", "");
                    this.simaiTrackInformation.Information["Easy Chart Maker"] = easyChartCandidate;
                }
                else if (item.Contains("lv_2"))
                {
                    string basicCandidate = item.Replace("lv_2=", "");
                    this.simaiTrackInformation.Information["Basic"] = basicCandidate;
                }
                else if (item.Contains("des_2"))
                {
                    string basicChartCandidate = item.Replace("des_2=", "");
                    this.simaiTrackInformation.Information["Basic Chart Maker"] = basicChartCandidate;
                }
                else if (item.Contains("lv_3"))
                {
                    string advancedCandidate = item.Replace("lv_3=", "");
                    this.simaiTrackInformation.Information["Advanced"] = advancedCandidate;
                }
                else if (item.Contains("des_3"))
                {
                    string advancedChartCandidate = item.Replace("des_3=", "");
                    this.simaiTrackInformation.Information["Advanced Chart Maker"] = advancedChartCandidate;
                }
                else if (item.Contains("lv_4"))
                {
                    string expertCandidate = item.Replace("lv_4=", "");
                    this.simaiTrackInformation.Information["Expert"] = expertCandidate;
                }
                else if (item.Contains("des_4"))
                {
                    string expertChartCandidate = item.Replace("des_4=", "");
                    this.simaiTrackInformation.Information["Expert Chart Maker"] = expertChartCandidate;
                }
                else if (item.Contains("lv_5"))
                {
                    string masterCandidate = item.Replace("lv_5=", "");
                    this.simaiTrackInformation.Information["Master"] = masterCandidate;
                }
                else if (item.Contains("des_5"))
                {
                    string masterChartCandidate = item.Replace("des_5=", "");
                    this.simaiTrackInformation.Information["Master Chart Maker"] = masterChartCandidate;
                }
                else if (item.Contains("lv_6"))
                {
                    string remasterCandidate = item.Replace("lv_6=", "");
                    this.simaiTrackInformation.Information["Remaster"] = remasterCandidate;
                }
                else if (item.Contains("des_6"))
                {
                    string remasterChartCandidate = item.Replace("des_6=", "");
                    this.simaiTrackInformation.Information["Remaster Chart Maker"] = remasterChartCandidate;
                }
                else if (item.Contains("lv_7"))
                {
                    string utageCandidate = item.Replace("lv_7=", "");
                    this.simaiTrackInformation.Information["Utage"] = utageCandidate;
                }
                else if (item.Contains("des_7"))
                {
                    string utageChartCandidate = item.Replace("des_7=", "");
                    this.simaiTrackInformation.Information["Utage Chart Maker"] = utageChartCandidate;
                }
                else if (item.Contains("inote_2"))
                {
                    string noteCandidate = item.Replace("inote_2=", "");
                    this.chartCandidates.Add("2", this.TokensFromText(noteCandidate));
                }
                else if (item.Contains("inote_3"))
                {
                    string noteCandidate = item.Replace("inote_3=", "");
                    this.chartCandidates.Add("3", this.TokensFromText(noteCandidate));
                }
                else if (item.Contains("inote_4"))
                {
                    string noteCandidate = item.Replace("inote_4=", "");
                    this.chartCandidates.Add("4", this.TokensFromText(noteCandidate));
                }
                else if (item.Contains("inote_5"))
                {
                    string noteCandidate = item.Replace("inote_5=", "");
                    this.chartCandidates.Add("5", this.TokensFromText(noteCandidate));
                }
                else if (item.Contains("inote_6"))
                {
                    string noteCandidate = item.Replace("inote_6=", "");
                    this.chartCandidates.Add("6", this.TokensFromText(noteCandidate));
                }
                else if (item.Contains("inote_7"))
                {
                    string noteCandidate = item.Replace("inote_7=", "");
                    this.chartCandidates.Add("7", this.TokensFromText(noteCandidate));
                }
            }
        }

        /// <summary>
        /// Update candidates from texts specified
        /// </summary>
        /// <param name="path">Location of text to be tokenized</param>
        public void UpdateFromPath(string path)
        {
            string[] takeIn = File.ReadAllLines(path);
            string storage = "";
            foreach (string line in takeIn)
            {
                storage += line;
            }
            this.UpdateFromText(storage);
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

