using System.Xml;

namespace MaichartConverter
{
    public abstract class TrackInformation : IXmlUtility
    {
        public static readonly string[] level = { "1", "2", "3", "4", "5", "6", "7", "7+", "8", "8+", "9", "9+", "10", "10+", "11", "11+", "12", "12+", "13", "13+", "14", "14+", "15", "15+" };
        public static readonly string[] version = { "maimai", "maimai PLUS", "maimai GreeN", "maimai GreeN PLUS", "maimai ORANGE", "maimai ORANGE PLUS", "maimai PiNK", "maimai PiNK PLUS", "maimai MURASAKi", "maimai MURASAKi PLUS", "maimai MiLK", "maimai MiLK PLUS", "maimai FiNALE", "maimai DX", "maimai DX PLUS", "maimai DX Splash", "maimai DX Splash PLUS", "maimai UNiVERSE" };
        private Dictionary<string, string> information;
        private XmlDocument takeInValue;

        public TrackInformation()
        {
            this.takeInValue = new XmlDocument();
            this.information = new Dictionary<string, string>
            {
                { "Name", "" },
                { "Sort Name", "" },
                { "Music ID", "" },
                { "Genre", "" },
                { "Version", "" },
                { "BPM", "" },
                { "Composer", "" },
                { "Easy", "" },
                { "Easy Chart Maker", "" },
                { "Easy Chart Path", "" },
                { "Basic", "" },
                { "Basic Chart Maker", "" },
                { "Basic Chart Path", "" },
                { "Advanced", "" },
                { "Advanced Chart Maker", "" },
                { "Advanced Chart Path", "" },
                { "Expert", "" },
                { "Expert Chart Maker", "" },
                { "Expert Chart Path", "" },
                { "Master", "" },
                { "Master Chart Maker", "" },
                { "Master Chart Path", "" },
                { "Remaster", "" },
                { "Remaster Chart Maker", "" },
                { "Remaster Chart Path", "" },
                { "Utage", "" },
                { "Utage Chart Maker", "" },
                { "Utage Chart Path", "" }
            };
            this.Update();
        }

        public TrackInformation(string location)
        {
            {
                this.takeInValue = new XmlDocument();
                if (File.Exists(location + "Music.xml"))
                {
                    this.takeInValue.Load(location + "Music.xml");
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
            get { return this.Information.GetValueOrDefault("Name") ?? throw new NullReferenceException("Name is not defined"); }
            set { this.information["Name"] = value; }
        }

        public string TrackSortName
        {
            get { return this.Information.GetValueOrDefault("Sort Name") ?? throw new NullReferenceException("Sort Name is not defined"); }
            set { this.information["Sort Name"] = value; }
        }

        public string TrackID
        {
            get { return this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not defined"); }
            set { this.information["Music ID"] = value; }
        }

        public string TrackGenre
        {
            get { return this.Information.GetValueOrDefault("Genre") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["Genre"] = value; }
        }

        public string TrackBPM
        {
            get { return this.Information.GetValueOrDefault("BPM") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["BPM"] = value; }
        }

        public string TrackComposer
        {
            get { return this.Information.GetValueOrDefault("Composer") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["Composer"] = value; }
        }

        public string DXChart
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not Defined");
                if (musicID.Length > 3)
                    return "_DX";
                else return "";
            }
        }

        public bool IsDXChart
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not Defined");
                return musicID.Length > 3;
            }
        }

        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeInValue.GetElementsByTagName(name);
            return result;
        }

        public string TrackVersion
        {

            get
            {
                string version = this.Information.GetValueOrDefault("Version") ?? throw new NullReferenceException("Version is not Defined");
                return version;
            }
            set { this.information["Version"] = value; }
        }

        public XmlDocument TakeInValue
        {
            get { return takeInValue; }
            set { this.takeInValue = value; }
        }

        public Dictionary<string, string> Information
        {
            get { return information; }
            set { this.information = value; }
        }

        public void Save(string location)
        {
            this.takeInValue.Save(location);
        }

        public abstract void Update();
    }
}