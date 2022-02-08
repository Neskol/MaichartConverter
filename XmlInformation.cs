using System.Xml;

namespace MaichartConverter
{
    public class XmlInformation :TrackInformation, IXmlUtility
    {
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

        public override void Update()
        {
            XmlNodeList nameCandidate = this.TakeInValue.GetElementsByTagName("name");
            XmlNodeList bpmCandidate = this.TakeInValue.GetElementsByTagName("bpm");
            XmlNodeList chartCandidate = this.TakeInValue.GetElementsByTagName("Notes");
            XmlNodeList composerCandidate = this.TakeInValue.GetElementsByTagName("artistName");
            XmlNodeList genreCandidate = this.TakeInValue.GetElementsByTagName("genreName");
            XmlNodeList addVersionCandidate = this.TakeInValue.GetElementsByTagName("AddVersion");
            XmlNodeList sortNameCandidate = this.TakeInValue.GetElementsByTagName("sortName");
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
                        this.Information["ReRemaster"]= level[Int32.Parse(musicLevelIDCandidate.InnerText) - 1];
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
        }
    }
}
