using System.Xml;

namespace MaidataConverter
{
    internal class XmlInformation : IXmlUtility
    {
        public static readonly string[] level = { "1", "2", "3", "4", "5", "6", "7", "7+", "8", "8+", "9", "9+", "10", "10+", "11", "11+", "12", "12+", "13", "13+", "14", "14+", "15", "15+" };
        public static readonly string[] version = {"maimai","maimai PLUS","maimai GreeN","maimai GreeN PLUS","maimai ORANGE","maimai ORANGE PLUS","maimai PiNK","maimai PiNK PLUS","maimai MURASAKi","maimai MURASAKi PLUS","maimai MiLK","maimai MiLK PLUS","maimai FiNALE","maimai DX","maimai DX PLUS","maimai DX Splash","maimai DX Splash PLUS","maimai UNiVERSE" };
        private Dictionary<string, string> information;
        private XmlDocument takeinValue;

        public XmlInformation()
        {
            takeinValue = new XmlDocument();
            information = new Dictionary<string, string>();
            this.Update();
        }

        public XmlInformation(string location)
        {
            {
                this.takeinValue = new XmlDocument();
                if (File.Exists(location+"Music.xml"))
                {
                    this.takeinValue.Load(location + "Music.xml");
                    this.information = new Dictionary<string, string>();
                    this.Update();
                }
                else
                {
                    this.information = new Dictionary<string, string>();
                }
                
            }
        }

        public string TrackName
        {
            get { return this.Information.GetValueOrDefault("Name")??throw new NullReferenceException("Name is not defined");}
        }

        public string TrackSortName
        {
            get { return this.Information.GetValueOrDefault("Sort Name")??throw new NullReferenceException("Sort Name is not defined"); }
        }

        public string TrackID
        {
            get { return this.Information.GetValueOrDefault("Music ID")??throw new NullReferenceException("Music ID is not defined"); }
        }

        public string TrackGenre
        {
            get { return this.Information.GetValueOrDefault("Genre")??throw new NullReferenceException("Genre is not defined"); }
        }

        public string DXChart
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID")??throw new NullReferenceException("Music ID is not Defined");
                if (musicID.Length > 3)
                    return "_DX";
                else return "";
            }
        }

        public bool IsDXChart
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID")??throw new NullReferenceException("Music ID is not Defined");
                return musicID.Length > 3;
            }
        }

        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeinValue.GetElementsByTagName(name);
            return result;
        }

        public string Version
        {
            
            get { string version = this.Information.GetValueOrDefault("Version")??throw new NullReferenceException("Version is not Defined");
                return version; }
        }

        public XmlDocument TakeinValue
        {
            get { return takeinValue; }
        }

        public Dictionary<string, string> Information
        {
            get { return information; }
        }

        public void Save(string location)
        {
            this.takeinValue.Save(location);
        }

        public void Update()
        {
            XmlNodeList nameCandidate = takeinValue.GetElementsByTagName("name");
            XmlNodeList bpmCandidate = takeinValue.GetElementsByTagName("bpm");
            XmlNodeList chartCandidate = takeinValue.GetElementsByTagName("Notes");
            XmlNodeList composerCandidate = takeinValue.GetElementsByTagName("artistName");
            XmlNodeList genreCandidate = takeinValue.GetElementsByTagName("genreName");
            XmlNodeList addVersionCandidate = takeinValue.GetElementsByTagName("AddVersion");
            XmlNodeList sortNameCandidate = takeinValue.GetElementsByTagName("sortName");
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
                    if (candidate["file"]["path"].InnerText.Contains("00.ma2")&&candidate["isEnable"].InnerText.Equals("true"))
                    {
                        this.information.Add("Basic", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Basic Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Basic Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("01.ma2") && candidate["isEnable"].InnerText.Equals("true"))
                    {
                        this.information.Add("Advanced", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Advanced Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Advanced Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("02.ma2") && candidate["isEnable"].InnerText.Equals("true"))
                    {
                        this.information.Add("Expert", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Expert Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Expert Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("03.ma2") && candidate["isEnable"].InnerText.Equals("true"))
                    {
                        this.information.Add("Master", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Master Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Master Chart Path", candidate["file"]["path"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains("04.ma2") && candidate["isEnable"].InnerText.Equals("true"))
                    {
                        this.information.Add("Remaster", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
                        this.information.Add("Remaster Chart Maker", candidate["notesDesigner"]["str"].InnerText);
                        this.information.Add("Remaster Chart Path", candidate["file"]["path"].InnerText);
                    }
                }
                catch (Exception ex)
                {
                   Console.WriteLine("There is no such chart: " + ex.Message);
                   throw ex;
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

            foreach (XmlNode candidate in sortNameCandidate)
            {
                {
                    if (!this.information.ContainsKey("Sort Name"))
                    {
                        this.information.Add("Sort Name", candidate.InnerText);
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

            foreach (XmlNode candidate in addVersionCandidate)
            {
                {
                    if (!this.information.ContainsKey("AddVersion"))
                    {
                        this.information.Add("Version", version[Int32.Parse(candidate["id"].InnerText)]);
                    }
                }
            }
        }
    }
}
