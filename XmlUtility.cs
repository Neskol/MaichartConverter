using System.Xml;

namespace MusicConverterTest
{
    internal class XmlUtility : IXmlUtility
    {
        public static readonly string[] level = { "1", "2", "3", "4", "5", "6", "7", "7+", "8", "8+", "9", "9+", "10", "10+", "11", "11+", "12", "12+", "13", "13+", "14", "14+", "15", "15+" };
        private Dictionary<string, string> information;
        private XmlDocument takeinValue;

        public XmlUtility()
        {
            takeinValue = new XmlDocument();
            information = new Dictionary<string, string>();
            this.Update();
        }

        public XmlUtility(string location)
        {
            try
            {
                this.takeinValue = new XmlDocument();
                this.takeinValue.Load(location + "Music.xml");
                this.information = new Dictionary<string, string>();
                this.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string TrackName
        {
            get { return this.Information.GetValueOrDefault("Name"); }
        }

        public string TrackID
        {
            get { return this.Information.GetValueOrDefault("Music ID"); }
        }

        public string TrackGenre
        {
            get { return this.Information.GetValueOrDefault("Genre"); }
        }

        public string DXChart
        {
            get
            {
                if (this.Information.GetValueOrDefault("Music ID").Length > 3)
                    return "_DX";
                else return "";
            }
        }

        public bool IsDXChart
        {
            get
            {
                return this.Information.GetValueOrDefault("Music ID").Length > 3;
            }
        }

        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeinValue.GetElementsByTagName(name);
            return result;
        }

        public XmlDocument TakeinValue
        {
            get { return takeinValue; }
        }

        public Dictionary<string, string> Information
        {
            get { return information; }
        }

        //public void Load(string location)
        //{
        //    this.takeinValue = new XmlDocument();
        //    this.takeinValue.LoadXml(location);
        //    this.information = new Dictionary<string, string>();
        //}

        public void Save(string location)
        {
            this.takeinValue.Save(location);
        }

        public void Update()
        {
            //try
            //{
            XmlNodeList nameCandidate = takeinValue.GetElementsByTagName("name");
            XmlNodeList bpmCandidate = takeinValue.GetElementsByTagName("bpm");
            XmlNodeList chartCandidate = takeinValue.GetElementsByTagName("Notes");
            XmlNodeList composerCandidate = takeinValue.GetElementsByTagName("artistName");
            XmlNodeList genreCandidate = takeinValue.GetElementsByTagName("genreName");
            //Add in name and music ID.
            ////Add BPM
            //this.information.Add("BPM",bpmCandidate[0].InnerText);
            foreach (XmlNode candidate in nameCandidate)
            {
                if (!this.information.ContainsKey("Music ID"))
                {
                    this.information.Add("Music ID", candidate["id"].InnerText);
                    this.information.Add("Name", candidate["str"].InnerText);
                }
            }
            foreach (XmlNode candidate in chartCandidate)
            {
                try
                {
                    if (candidate["file"]["path"].InnerText.Contains("00.ma2"))
                    {
                        this.information.Add("Basic", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Basic Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Basic Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("01.ma2"))
                    {
                        this.information.Add("Advanced", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Advanced Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Advanced Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("02.ma2"))
                    {
                        this.information.Add("Expert", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Expert Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Expert Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("03.ma2"))
                    {
                        this.information.Add("Master", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Master Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Master Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("04.ma2"))
                    {
                        this.information.Add("Remaster", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Remaster Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Remaster Chart Path", candidate["file"]["path"].InnerText);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There is no such chart: " + ex.Message);
                }
            }

            foreach (XmlNode candidate in bpmCandidate)
            {
                {
                    if (!this.information.ContainsKey("BPM"))
                    {
                        this.information.Add("BPM", candidate.InnerText);
                    }
                }
            }

            foreach (XmlNode candidate in composerCandidate)
            {
                {
                    if (!this.information.ContainsKey("Composer"))
                    {
                        this.information.Add("Composer", candidate["str"].InnerText);
                    }
                }
            }

            foreach (XmlNode candidate in genreCandidate)
            {
                {
                    if (!this.information.ContainsKey("Genre"))
                    {
                        this.information.Add("Genre", candidate["str"].InnerText);
                    }
                }
            }

            //}catch (Exception e)
            //{
            //    throw e;
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
