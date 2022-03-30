using System.Xml;
using System.Xml.Linq;

namespace MaichartConverter
{
    /// <summary>
    /// Use xml to store track information
    /// </summary>
    public abstract class TrackInformation : IXmlUtility
    {
        /// <summary>
        /// Stores proper difficulties
        /// </summary>
        /// <value>1-15 Maimai level</value>
        public static readonly string[] level = { "1", "2", "3", "4", "5", "6", "7", "7+", "8", "8+", "9", "9+", "10", "10+", "11", "11+", "12", "12+", "13", "13+", "14", "14+", "15", "15+" };

        /// <summary>
        /// Stores prover maimai versions
        /// </summary>
        /// <value>Version name of each generation of Maimai</value>
        public static readonly string[] version = { "maimai", "maimai PLUS", "maimai GreeN", "maimai GreeN PLUS", "maimai ORANGE", "maimai ORANGE PLUS", "maimai PiNK", "maimai PiNK PLUS", "maimai MURASAKi", "maimai MURASAKi PLUS", "maimai MiLK", "maimai MiLK PLUS", "maimai FiNALE", "maimai DX", "maimai DX PLUS", "maimai DX Splash", "maimai DX Splash PLUS", "maimai DX UNiVERSE", "maimai DX UNiVERSE PLUS" };

        /// <summary>
        /// Set of track information stored
        /// </summary>
        private Dictionary<string, string> information;

        /// <summary>
        /// Internal stored information set
        /// </summary>
        private XmlDocument takeInValue;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TrackInformation()
        {
            this.takeInValue = new XmlDocument();
            this.information = new Dictionary<string, string>();
            this.FormatInformation();
            this.Update();
        }

        // /// <summary>
        // /// Construct track information from given location
        // /// </summary>
        // /// <param name="location">Place to load</param>
        // public TrackInformation(string location)
        // {
        //     {
        //         this.takeInValue = new XmlDocument();
        //         if (File.Exists(location + "Music.xml"))
        //         {
        //             this.takeInValue.Load(location + "Music.xml");
        //             this.information=new Dictionary<string, string>();
        //             this.FormatInformation();
        //             this.Update();
        //         }
        //         else
        //         {
        //             this.information=new Dictionary<string, string>();
        //             this.FormatInformation();
        //         }

        //     }
        // }

        /// <summary>
        /// Add in necessary nodes in information.
        /// </summary>
        public void FormatInformation()
        {
            this.information = new Dictionary<string, string>
                    {
                        { "Name", "" },
                        { "Sort Name", "" },
                        { "Music ID", "" },
                        { "Genre", "" },
                        { "Version", "" },
                        {"Version Number",""},
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
                        { "Utage Chart Path", "" },
                        {"SDDX Suffix",""}
                    };
        }

        /// <summary>
        /// Return the track name
        /// </summary>
        /// <value>this.TrackName</value>
        public string TrackName
        {
            get { return this.Information.GetValueOrDefault("Name") ?? throw new NullReferenceException("Name is not defined"); }
            set { this.information["Name"] = value; }
        }

        /// <summary>
        /// Return the sort name (basically English or Katakana)
        /// </summary>
        /// <value>this.SortName</value>
        public string TrackSortName
        {
            get { return this.Information.GetValueOrDefault("Sort Name") ?? throw new NullReferenceException("Sort Name is not defined"); }
            set { this.information["Sort Name"] = value; }
        }

        /// <summary>
        /// Return the track ID (4 digit, having 00 for SD 01 for DX)
        /// </summary>
        /// <value>this.TrackID</value>
        public string TrackID
        {
            get { return this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not defined"); }
            set { this.information["Music ID"] = value; }
        }

        /// <summary>
        /// Return the track genre (By default cabinet 6 categories)
        /// </summary>
        /// <value>this.TrackGenre</value>
        public string TrackGenre
        {
            get { return this.Information.GetValueOrDefault("Genre") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["Genre"] = value; }
        }

        /// <summary>
        /// Return the track global BPM
        /// </summary>
        /// <value>this.TrackBPM</value>
        public string TrackBPM
        {
            get { return this.Information.GetValueOrDefault("BPM") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["BPM"] = value; }
        }

        /// <summary>
        /// Return the track composer
        /// </summary>
        /// <value>this.TrackComposer</value>
        public string TrackComposer
        {
            get { return this.Information.GetValueOrDefault("Composer") ?? throw new NullReferenceException("Genre is not defined"); }
            set { this.information["Composer"] = value; }
        }

        /// <summary>
        /// Return the most representative level of the track = by default master
        /// </summary>
        /// <value>this.MasterTrackLevel</value>
        public string TrackSymbolicLevel
        {
            get { return this.Information.GetValueOrDefault("Master") ?? throw new NullReferenceException("Master level is not defined"); }
            set { this.information["Master"] = value; }
        }

        /// <summary>
        /// Return the suffix of Track title for export
        /// </summary>
        /// <value>this.TrackSubstituteName"_DX" if is DX chart</value>
        public string DXChartTrackPathSuffix
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not Defined");
                if (musicID.Length > 3)
                    return "_DX";
                else return "";
            }
        }

        /// <summary>
        /// Returns if the track is Standard or Deluxe
        /// </summary>
        /// <value>SD if standard, DX if deluxe</value>
        public string StandardDeluxePrefix
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not Defined");
                if (musicID.Length > 3)
                    return "DX";
                else return "SD";
            }
        }

        /// <summary>
        /// Title suffix for better distinguish
        /// </summary>
        /// <value>[SD] if Standard and [DX] if Deluxe</value>
        public string StandardDeluxeSuffix
        {
            get
            {
                return "[" + this.StandardDeluxePrefix + "]";
            }
        }

        /// <summary>
        /// See if the chart is DX chart.
        /// </summary>
        /// <value>True if is DX, false if SD</value>
        public bool IsDXChart
        {
            get
            {
                string musicID = this.Information.GetValueOrDefault("Music ID") ?? throw new NullReferenceException("Music ID is not Defined");
                return musicID.Length > 3;
            }
        }

        /// <summary>
        /// Return the XML node that has same name with
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeInValue.GetElementsByTagName(name);
            return result;
        }

        /// <summary>
        /// Return this.TrackVersion
        /// </summary>
        /// <value>this.TrackVersion</value>
        public string TrackVersion
        {

            get
            {
                string version = this.Information.GetValueOrDefault("Version") ?? throw new NullReferenceException("Version is not Defined");
                return version;
            }
            set { this.information["Version"] = value; }
        }

        /// <summary>
        /// Return this.TrackVersionNumber
        /// </summary>
        /// <value>this.TrackVersionNumber</value>
        public string TrackVersionNumber
        {

            get
            {
                string versionNumber = this.Information.GetValueOrDefault("Version Number") ?? throw new NullReferenceException("Version is not Defined");
                return versionNumber;
            }
            set { this.information["Version Number"] = value; }
        }

        /// <summary>
        /// Give access to TakeInValue if necessary
        /// </summary>
        /// <value>this.TakeInValue as XMLDocument</value>
        public XmlDocument TakeInValue
        {
            get { return takeInValue; }
            set { this.takeInValue = value; }
        }

        /// <summary>
        /// Give access to this.Information
        /// </summary>
        /// <value>this.Information as Dictionary</value>
        public Dictionary<string, string> Information
        {
            get { return information; }
            set { this.information = value; }
        }

        /// <summary>
        /// Save the information to given path
        /// </summary>
        /// <param name="location">Path to save the information</param>
        public void Save(string location)
        {
            this.takeInValue.Save(location);
        }

        /// <summary>
        /// Update information
        /// </summary>
        public abstract void Update();
    }
}