using System.Xml;

namespace MusicConverterTest
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
#pragma warning disable CS8603 // 可能返回 null 引用。
            get { return this.Information.GetValueOrDefault("Name"); }
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        public string TrackID
        {
#pragma warning disable CS8603 // 可能返回 null 引用。
            get { return this.Information.GetValueOrDefault("Music ID"); }
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        public string TrackGenre
        {
#pragma warning disable CS8603 // 可能返回 null 引用。
            get { return this.Information.GetValueOrDefault("Genre"); }
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        public string DXChart
        {
            get
            {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                if (this.Information.GetValueOrDefault("Music ID").Length > 3)
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    return "_DX";
                else return "";
            }
        }

        public bool IsDXChart
        {
            get
            {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                return this.Information.GetValueOrDefault("Music ID").Length > 3;
#pragma warning restore CS8602 // 解引用可能出现空引用。
            }
        }

        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeinValue.GetElementsByTagName(name);
            return result;
        }

        public string Version
        {
            get {return this.information.GetValueOrDefault("Version"); }
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
            XmlNodeList addVersionCandidate = takeinValue.GetElementsByTagName("AddVersion");
            //Add in name and music ID.
            ////Add BPM
            //this.information.Add("BPM",bpmCandidate[0].InnerText);
            foreach (XmlNode candidate in nameCandidate)
            {
                if (!this.information.ContainsKey("Music ID"))
                {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    this.information.Add("Music ID", candidate["id"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    this.information.Add("Name", candidate["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                }
            }
            foreach (XmlNode candidate in chartCandidate)
            {
                try
                {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    if (candidate["file"]["path"].InnerText.Contains("00.ma2"))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Basic", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Basic Chart Maker", candidate["notesDesigner"]["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Basic Chart Path", candidate["file"]["path"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    else if (candidate["file"]["path"].InnerText.Contains("01.ma2"))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Advanced", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Advanced Chart Maker", candidate["notesDesigner"]["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Advanced Chart Path", candidate["file"]["path"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    else if (candidate["file"]["path"].InnerText.Contains("02.ma2"))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Expert", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Expert Chart Maker", candidate["notesDesigner"]["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Expert Chart Path", candidate["file"]["path"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    else if (candidate["file"]["path"].InnerText.Contains("03.ma2"))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Master", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Master Chart Maker", candidate["notesDesigner"]["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Master Chart Path", candidate["file"]["path"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    else if (candidate["file"]["path"].InnerText.Contains("04.ma2"))
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Remaster", level[Int32.Parse(candidate["musicLevelID"].InnerText) - 1]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Remaster Chart Maker", candidate["notesDesigner"]["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Remaster Chart Path", candidate["file"]["path"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
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
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Composer", candidate["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
                }
            }

            foreach (XmlNode candidate in genreCandidate)
            {
                {
                    if (!this.information.ContainsKey("Genre"))
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Genre", candidate["str"].InnerText);
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    }
                }
            }

            foreach (XmlNode candidate in addVersionCandidate)
            {
                {
                    if (!this.information.ContainsKey("AddVersion"))
                    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                        this.information.Add("Version", version[Int32.Parse(candidate["id"].InnerText)]);
#pragma warning restore CS8602 // 解引用可能出现空引用。
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
