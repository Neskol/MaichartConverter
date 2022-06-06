using System.Xml;

namespace MaichartConverter
{
    /// <summary>
    /// Using Xml to store trackInformation
    /// </summary>
    public class XmlInformation :TrackInformation, IXmlUtility
    {
        /// <summary>
        /// Using take in Xml to store trackInformation:
        /// </summary>
        public XmlInformation()
        {
            this.Update();
        }

        public XmlInformation(string location)
        {
            {
                if (File.Exists(location+"Music.xml"))
                {
                    this.TakeInValue.Load(location + "Music.xml");
                    this.Update();
                }
                else
                {
                    this.Update();
                }
                
            }
        }

        /// <summary>
        /// Generate new music.xml for export
        /// </summary>
        public void GenerateEmptyStoredXML()
        {
            this.TakeInValue = new XmlDocument();
            //Create declaration
            XmlDeclaration dec = this.TakeInValue.CreateXmlDeclaration("1.0","utf-8","yes");
            this.TakeInValue.AppendChild(dec);
            //Create Root and append attributes
            XmlElement root = this.TakeInValue.CreateElement("MusicData");
            XmlAttribute xsi = this.TakeInValue.CreateAttribute("xmlns:xsi");
            xsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            XmlAttribute xsd = this.TakeInValue.CreateAttribute("xmlns:xsd");
            xsd.Value = "http://www.w3.org/2001/XMLSchema";
            root.AppendChild(xsi);
            root.AppendChild(xsd);

            //Create tags. *data name: inner text = music0xxxxx
            XmlElement dataName = this.TakeInValue.CreateElement("dataName");
            XmlElement netOpenName = this.TakeInValue.CreateElement("netOpenName");
            XmlElement netOpenNameId = this.TakeInValue.CreateElement("id");
            XmlElement netOpenNameStr = this.TakeInValue.CreateElement("str");
            XmlElement releaseTagName = this.TakeInValue.CreateElement("releaseTagName");
            XmlElement releaseTagNameId = this.TakeInValue.CreateElement("id");
            XmlElement releaseTagNameStr = this.TakeInValue.CreateElement("str");
            XmlElement disable = this.TakeInValue.CreateElement("disable");
            disable.InnerText = "false";
            XmlElement name = this.TakeInValue.CreateElement("name");
            XmlElement nameId = this.TakeInValue.CreateElement("id");
            nameId.InnerText = this.TrackID;
            XmlElement nameStr = this.TakeInValue.CreateElement("str");
            nameStr.InnerText = this.TrackName;
            XmlElement rightsInfoName = this.TakeInValue.CreateElement("rightsInfoName");
            XmlElement rightsInfoNameId = this.TakeInValue.CreateElement("id");
            XmlElement rightsInfoNameStr = this.TakeInValue.CreateElement("str");
            XmlElement sortName = this.TakeInValue.CreateElement("sortName"); 
            XmlElement artistName = this.TakeInValue.CreateElement("artistName");
            XmlElement artistNameId = this.TakeInValue.CreateElement("id");
            XmlElement artistNameStr = this.TakeInValue.CreateElement("str");
            XmlElement genreName = this.TakeInValue.CreateElement("genreName");
            XmlElement genreNameId = this.TakeInValue.CreateElement("id");
            XmlElement genreNameStr = this.TakeInValue.CreateElement("str");
            genreNameStr.InnerText = this.TrackGenre;
            XmlElement bpm = this.TakeInValue.CreateElement("bpm");
            bpm.InnerText = this.TrackBPM;
            XmlElement version = this.TakeInValue.CreateElement("version");
            XmlElement versionId = this.TakeInValue.CreateElement("id");
            XmlElement versionStr = this.TakeInValue.CreateElement("str");
            XmlElement addVersion = this.TakeInValue.CreateElement("addVersion");
            XmlElement addVersionId = this.TakeInValue.CreateElement("id");
            addVersionId.InnerText = this.TrackVersionNumber;
            XmlElement addVersionStr = this.TakeInValue.CreateElement("str");
            addVersionStr.InnerText = this.TrackVersion;
            XmlElement movieName = this.TakeInValue.CreateElement("movieName");
            XmlElement movieNameId = this.TakeInValue.CreateElement("id");
            movieNameId.InnerText = this.TrackID;
            XmlElement movieNameStr = this.TakeInValue.CreateElement("str");
            XmlElement cueName = this.TakeInValue.CreateElement("cueName");
            XmlElement cueNameId = this.TakeInValue.CreateElement("id");
            cueNameId.InnerText = this.TrackID;
            XmlElement cueNameStr = this.TakeInValue.CreateElement("str");
            XmlElement dressCode = this.TakeInValue.CreateElement("dressCode");
            XmlElement eventName = this.TakeInValue.CreateElement("eventName");
            XmlElement eventNameId = this.TakeInValue.CreateElement("id");
            XmlElement eventNameStr = this.TakeInValue.CreateElement("str");
            XmlElement subEventName = this.TakeInValue.CreateElement("subEventName");
            XmlElement subEventNameId = this.TakeInValue.CreateElement("id");
            XmlElement subEventNameStr = this.TakeInValue.CreateElement("str");
            XmlElement lockType = this.TakeInValue.CreateElement("lockType");
            XmlElement subLockType = this.TakeInValue.CreateElement("subLockType");
            XmlElement dotNetListView = this.TakeInValue.CreateElement("dotNetListView");
            XmlElement notesData = this.TakeInValue.CreateElement("dataName");
            XmlElement jacketFile = this.TakeInValue.CreateElement("jacketFile");
            XmlElement thumbnailName = this.TakeInValue.CreateElement("thumbnailName");
            XmlElement rightFile = this.TakeInValue.CreateElement("rightFile");
            XmlElement priority = this.TakeInValue.CreateElement("priority");




        }

        public XmlElement CreateNotesInformation(Dictionary<string, string> information, int chartIndex)
        {
            XmlElement result = this.TakeInValue.CreateElement("Notes");

            return result;
        }

        public override void Update()
        {
            XmlNodeList nameCandidate = this.TakeInValue.GetElementsByTagName("name");
            XmlNodeList bpmCandidate = this.TakeInValue.GetElementsByTagName("bpm");
            XmlNodeList chartCandidate = this.TakeInValue.GetElementsByTagName("Notes");
            XmlNodeList composerCandidate = this.TakeInValue.GetElementsByTagName("artistName");
            XmlNodeList genreCandidate = this.TakeInValue.GetElementsByTagName("genreName");
            XmlNodeList addVersionCandidate = this.TakeInValue.GetElementsByTagName("AddVersion");
            XmlNodeList sortNameCandidate = this.TakeInValue.GetElementsByTagName("sortName");
            XmlNodeList versionNumberCandidate = this.TakeInValue.GetElementsByTagName("releaseTagName");
            //Add in name and music ID.
            ////Add BPM
            //this.information.Add("BPM",bpmCandidate[0].InnerText);
            foreach (XmlNode candidate in nameCandidate)
            {
                if (this.TrackID.Equals(""))
                {
                    var idCandidate = candidate["id"] ?? throw new NullReferenceException();
                    var strCandidate = candidate["str"] ?? throw new NullReferenceException();
                    this.TrackID= idCandidate.InnerText;
                    this.TrackName = strCandidate.InnerText;
                }
            }
            foreach (XmlNode candidate in chartCandidate)
            {
                try
                {
                    var pathCandidate = candidate["file"] ?? throw new NullReferenceException();
                    pathCandidate = pathCandidate["path"] ?? throw new NullReferenceException();
                    var enableCandidate = candidate["isEnable"] ?? throw new NullReferenceException();
                    if (pathCandidate.InnerText.Contains("00.ma2")&&enableCandidate.InnerText.Equals("true"))
                    {
                        var musicLevelIDCandidate = candidate["musicLevelID"] ?? throw new NullReferenceException();
                        var notesDesignerCandidate = candidate["notesDesigner"] ?? throw new NullReferenceException();
                        notesDesignerCandidate = notesDesignerCandidate["str"] ?? throw new NullReferenceException();
                        var fileCandidate = candidate["file"] ?? throw new NullReferenceException();
                        fileCandidate = fileCandidate["path"] ?? throw new NullReferenceException();
                        this.Information["Basic"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
                        this.Information["Basic Chart Maker"]= notesDesignerCandidate.InnerText;
                        this.Information["Basic Chart Path"]= fileCandidate.InnerText;
                    }
                    else if (pathCandidate.InnerText.Contains("01.ma2")&&enableCandidate.InnerText.Equals("true"))
                    {
                        var musicLevelIDCandidate = candidate["musicLevelID"] ?? throw new NullReferenceException();
                        var notesDesignerCandidate = candidate["notesDesigner"] ?? throw new NullReferenceException();
                        notesDesignerCandidate = notesDesignerCandidate["str"] ?? throw new NullReferenceException();
                        var fileCandidate = candidate["file"] ?? throw new NullReferenceException();
                        fileCandidate = fileCandidate["path"] ?? throw new NullReferenceException();
                        this.Information["Advanced"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
                        this.Information["Advanced Chart Maker"]= notesDesignerCandidate.InnerText;
                        this.Information["Advanced Chart Path"]= fileCandidate.InnerText;
                    }
                    else if (pathCandidate.InnerText.Contains("02.ma2")&&enableCandidate.InnerText.Equals("true"))
                    {
                        var musicLevelIDCandidate = candidate["musicLevelID"] ?? throw new NullReferenceException();
                        var notesDesignerCandidate = candidate["notesDesigner"] ?? throw new NullReferenceException();
                        notesDesignerCandidate = notesDesignerCandidate["str"] ?? throw new NullReferenceException();
                        var fileCandidate = candidate["file"] ?? throw new NullReferenceException();
                        fileCandidate = fileCandidate["path"] ?? throw new NullReferenceException();
                        this.Information["Expert"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
                        this.Information["Expert Chart Maker"]= notesDesignerCandidate.InnerText;
                        this.Information["Expert Chart Path"]= fileCandidate.InnerText;
                    }
                    else if (pathCandidate.InnerText.Contains("03.ma2")&&enableCandidate.InnerText.Equals("true"))
                    {
                        var musicLevelIDCandidate = candidate["musicLevelID"] ?? throw new NullReferenceException();
                        var notesDesignerCandidate = candidate["notesDesigner"] ?? throw new NullReferenceException();
                        notesDesignerCandidate = notesDesignerCandidate["str"] ?? throw new NullReferenceException();
                        var fileCandidate = candidate["file"] ?? throw new NullReferenceException();
                        fileCandidate = fileCandidate["path"] ?? throw new NullReferenceException();
                        this.Information["Master"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
                        this.Information["Master Chart Maker"]= notesDesignerCandidate.InnerText;
                        this.Information["Master Chart Path"]= fileCandidate.InnerText;
                    }
                    else if (pathCandidate.InnerText.Contains("04.ma2")&&enableCandidate.InnerText.Equals("true"))
                    {
                        var musicLevelIDCandidate = candidate["musicLevelID"] ?? throw new NullReferenceException();
                        var notesDesignerCandidate = candidate["notesDesigner"] ?? throw new NullReferenceException();
                        notesDesignerCandidate = notesDesignerCandidate["str"] ?? throw new NullReferenceException();
                        var fileCandidate = candidate["file"] ?? throw new NullReferenceException();
                        fileCandidate = fileCandidate["path"] ?? throw new NullReferenceException();
                        this.Information["Remaster"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
                        this.Information["Remaster Chart Maker"]= notesDesignerCandidate.InnerText;
                        this.Information["Remaster Chart Path"]= fileCandidate.InnerText;
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
                    if (this.TrackBPM.Equals(""))
                    {
                        this.TrackBPM= candidate.InnerText;
                    }
                }
            }

            foreach (XmlNode candidate in sortNameCandidate)
            {
                {
                    if (this.TrackSortName.Equals(""))
                    {
                        this.TrackSortName= candidate.InnerText;
                    }
                }
            }

            foreach (XmlNode candidate in composerCandidate)
            {
                {
                    if (this.TrackComposer.Equals(""))
                    {
                        var strCandidate = candidate["str"] ?? throw new NullReferenceException();
                        this.TrackComposer= strCandidate.InnerText;
                    }
                }
            }

            foreach (XmlNode candidate in genreCandidate)
            {
                {
                    if (this.TrackGenre.Equals(""))
                    {
                        var strCandidate = candidate["str"] ?? throw new NullReferenceException();
                        this.TrackGenre= strCandidate.InnerText;
                    }
                }
            }

            foreach (XmlNode candidate in versionNumberCandidate)
            {
                {
                    if (this.TrackVersionNumber.Equals(""))
                    {
                        var strCandidate = candidate["str"] ?? throw new NullReferenceException();
                        this.TrackVersionNumber = strCandidate.InnerText;
                    }
                }
            }

            foreach (XmlNode candidate in addVersionCandidate)
            {
                {
                    if (this.TrackVersion.Equals(""))
                    {
                        var idCandidate = candidate["id"] ?? throw new NullReferenceException();
                        this.TrackVersion= version[Int32.Parse(idCandidate.InnerText)];
                    }
                }
            }
            this.Information["SDDX Suffix"]=this.StandardDeluxeSuffix;
        }
    }
}
