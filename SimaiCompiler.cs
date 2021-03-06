using System.Collections.Immutable;
using System.ComponentModel;
using System.Xml;

namespace MaichartConverter
{
    /// <summary>
    /// Compile various Ma2 charts
    /// </summary>
    public class SimaiCompiler : ICompiler
    {
        /// <summary>
        /// Store difficulty keywords
        /// </summary>
        /// <value>Difficulty</value>
        public static readonly string[] difficulty = { "Basic", "Advanced", "Expert", "Master", "Remaster", "Utage" };

        /// <summary>
        /// Store chart collections
        /// </summary>
        private List<Chart> charts;

        /// <summary>
        /// Store global information
        /// </summary>
        private Dictionary<string, string> information;

        /// <summary>
        /// Store read in music XML file
        /// </summary>
        private XmlInformation musicXml;

        /// <summary>
        /// Construct compiler of a single song.
        /// </summary>
        /// <param name="location">Folder</param>
        /// <param name="targetLocation">Output folder</param>
        public SimaiCompiler(string location, string targetLocation)
        {
            charts = new List<Chart>();
            for (int i = 0; i < 5; i++)
            {
                charts.Add(new Ma2());
            }
            this.musicXml = new XmlInformation(location);
            this.information = musicXml.Information;
            //Construct charts
            {
                if (!this.information["Basic"].Equals(""))
                {
                    //Console.WriteLine("Have basic: "+ location + this.information.GetValueOrDefault("Basic Chart Path"));
                    charts[0] = new Ma2(location + this.information.GetValueOrDefault("Basic Chart Path"));
                }
                if (!this.information["Advanced"].Equals(""))
                {
                    charts[1] = new Ma2(location + this.information.GetValueOrDefault("Advanced Chart Path"));
                }
                if (!this.information["Expert"].Equals(""))
                {
                    charts[2] = new Ma2(location + this.information.GetValueOrDefault("Expert Chart Path"));
                }
                if (!this.information["Master"].Equals(""))
                {
                    charts[3] = new Ma2(location + this.information.GetValueOrDefault("Master Chart Path"));
                }
                if (!this.information["Remaster"].Equals(""))
                {
                    charts[4] = new Ma2(location + this.information.GetValueOrDefault("Remaster Chart Path"));
                }
            }
            string result = this.Compose();
            //Console.WriteLine(result);
            StreamWriter sw = new StreamWriter(targetLocation + Program.GlobalSep + "maidata.txt", false);
            {
                sw.WriteLine(result);
            }
            sw.Close();
        }

        /// <summary>
        /// Construct compiler of a single song.
        /// </summary>
        /// <param name="location">Folder</param>
        /// <param name="targetLocation">Output folder</param>
        /// <param name="forUtage">True if for utage</param>
        public SimaiCompiler(string location, string targetLocation, bool forUtage)
        {
            string[] ma2files = Directory.GetFiles(location, "*.ma2");
            charts = new List<Chart>();
            this.musicXml = new XmlInformation(location);
            this.information = musicXml.Information;
            foreach (string ma2file in ma2files)
            {
                charts.Add(new Ma2(ma2file));
            }

            List<string> ma2List = new List<string>();
            ma2List.AddRange(ma2files);

            string result = this.Compose(true, ma2List);
            //Console.WriteLine(result);
            StreamWriter sw = new StreamWriter(targetLocation + Program.GlobalSep + "maidata.txt", false);
            {
                sw.WriteLine(result);
            }
            sw.Close();
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public SimaiCompiler()
        {
            charts = new List<Chart>();
            information = new Dictionary<string, string>();
            this.musicXml = new XmlInformation();
        }

        public bool CheckValidity()
        {
            bool result = true;
            foreach (Chart x in charts)
            {
                result = result && x.CheckValidity();
            }
            return result;
        }

        public string Compose()
        {
            string result = "";
            //Add information
            {
                string beginning = "";
                beginning += "&title=" + this.information.GetValueOrDefault("Name") + this.information.GetValueOrDefault("SDDX Suffix") + "\n";
                beginning += "&wholebpm=" + this.information.GetValueOrDefault("BPM") + "\n";
                beginning += "&artist=" + this.information.GetValueOrDefault("Composer") + "\n";
                beginning += "&des=" + this.information.GetValueOrDefault("Master Chart Maker") + "\n";
                beginning += "&shortid=" + this.information.GetValueOrDefault("Music ID") + "\n";
                beginning += "&genre=" + this.information.GetValueOrDefault("Genre") + "\n";
                beginning += "&cabinet=";
                if (this.musicXml.IsDXChart)
                {
                    beginning += "DX\n";
                }
                else
                {
                    beginning += "SD\n";
                }
                beginning += "&version=" + this.musicXml.TrackVersion + "\n";
                beginning += "&chartconverter=Neskol\n";
                beginning += "\n";


                if (this.information.TryGetValue("Basic", out string? basic) && this.information.TryGetValue("Basic Chart Maker", out string? basicMaker))


                {
                    beginning += "&lv_2=" + basic + "\n";
                    beginning += "&des_2=" + basicMaker + "\n";
                    beginning += "\n";
                }


                if (this.information.TryGetValue("Advanced", out string? advance) && this.information.TryGetValue("Advanced Chart Maker", out string? advanceMaker))
                {
                    beginning += "&lv_3=" + advance + "\n";
                    beginning += "&des_3=" + advanceMaker + "\n";
                    beginning += "\n";
                }


                if (this.information.TryGetValue("Expert", out string? expert) && this.information.TryGetValue("Expert Chart Maker", out string? expertMaker))
                {
                    beginning += "&lv_4=" + expert + "\n";
                    beginning += "&des_4=" + expertMaker + "\n";
                    beginning += "\n";
                }


                if (this.information.TryGetValue("Master", out string? master) && this.information.TryGetValue("Master Chart Maker", out string? masterMaker))
                {
                    beginning += "&lv_5=" + master + "\n";
                    beginning += "&des_5=" + masterMaker + "\n";
                    beginning += "\n";
                }


                if (this.information.TryGetValue("Remaster", out string? remaster) && this.information.TryGetValue("Remaster Chart Maker", out string? remasterMaker))
                {
                    beginning += "&lv_6=" + remaster + "\n";
                    beginning += "&des_6=" + remasterMaker; beginning += "\n";
                    beginning += "\n";
                }
                result += beginning;
            }
            Console.WriteLine("Finished writing header of " + this.information.GetValueOrDefault("Name"));

            //Compose charts
            {
                for (int i = 0; i < this.charts.Count; i++)
                {
                    // Console.WriteLine("Processing chart: " + i);
                    if (!this.information[difficulty[i]].Equals(""))
                    {
                        string? isDxChart = this.information.GetValueOrDefault("SDDX Suffix");
                        if (!charts[i].IsDXChart)
                        {
                            isDxChart = "";
                        }
                        result += "&inote_" + (i + 2) + "=\n";
                        result += this.Compose(charts[i]);
                        Program.CompiledChart.Add(this.information.GetValueOrDefault("Name") + isDxChart + " [" + difficulty[i] + "]");
                    }
                    result += "\n";
                }
            }
            Console.WriteLine("Finished composing.");
            return result;
        }

        /// <summary>
        /// Return compose of specified chart.
        /// </summary>
        /// <param name="chart">Chart to compose</param>
        /// <returns>Maidata of specified chart WITHOUT headers</returns>
        public string Compose(Chart chart)
        {
            string result = "";
            int delayBar = (chart.TotalDelay) / 384 + 2;
            //Console.WriteLine(chart.Compose());
            //foreach (BPMChange x in chart.BPMChanges.ChangeNotes)
            //{
            //    Console.WriteLine("BPM Change verified in " + x.Bar + " " + x.Tick + " of BPM" + x.BPM);
            //}
            List<Note> firstBpm = new List<Note>();
            foreach (Note bpm in chart.Notes)
            {
                if (bpm.NoteSpecificType.Equals("BPM"))
                {
                    firstBpm.Add(bpm);
                }
            }
            // if (firstBpm.Count > 1)
            // {
            //     chart.Chart[0][0] = firstBpm[1];
            // }
            foreach (List<Note> bar in chart.StoredChart)
            {
                Note lastNote = new MeasureChange();
                //result += bar[1].Bar;
                foreach (Note x in bar)
                {
                    switch (lastNote.NoteSpecificType)
                    {
                        case "MEASURE":
                            break;
                        case "BPM":
                            break;
                        case "TAP":
                            if (x.IsNote && ((!x.NoteSpecificType.Equals("SLIDE")) && x.Tick == lastNote.Tick && !x.NoteGenre.Equals("BPM")))
                            {
                                result += "/";
                            }
                            else result += ",";
                            break;
                        case "HOLD":
                            if (x.IsNote && (!x.NoteSpecificType.Equals("SLIDE")) && x.Tick == lastNote.Tick && !x.NoteGenre.Equals("BPM"))
                            {
                                result += "/";
                            }
                            else result += ",";
                            break;
                        case "SLIDE_START":
                            //if (x.IsNote() && x.NoteSpecificType().Equals("SLIDE"))
                            //{

                            //}
                            break;
                        case "SLIDE":
                            if (x.IsNote && (!x.NoteSpecificType.Equals("SLIDE")) && x.Tick == lastNote.Tick && !x.NoteGenre.Equals("BPM"))
                            {
                                result += "/";
                            }
                            else if (x.IsNote && x.NoteSpecificType.Equals("SLIDE") && x.Tick == lastNote.Tick && !x.NoteGenre.Equals("BPM"))
                            {
                                result += "*";
                            }
                            else result += ",";
                            break;
                        default:
                            result += ",";
                            break;
                    }
                    //if (x.Prev!=null&&x.Prev.NoteType.Equals("NST"))
                    //if (x.NoteGenre.Equals("SLIDE")&&x.SlideStart== null)
                    //{
                    //    result += Int32.Parse(x.Key) + 1;
                    //    result += "!";
                    //}
                    result += x.Compose(0);
                    lastNote = x;
                    //if (x.NoteGenre().Equals("BPM"))
                    //{
                    //    result+="("+ x.Bar + "_" + x.Tick + ")";
                    //}
                }
                result += ",\n";
            }
            //if (delayBar>0)
            //{
            //    Console.WriteLine("TOTAL DELAYED BAR: "+delayBar);
            //}
            for (int i = 0; i < delayBar + 1; i++)
            {
                result += "{1},\n";
            }
            result += "E\n";
            return result;
        }

        /// <summary>
        /// Compose utage charts
        /// </summary>
        /// <param name="isUtage">switch to produce utage</param>
        /// <returns>Corresponding utage chart</returns>
        public string Compose(bool isUtage, List<string> ma2files)
        {
            string result = "";
            //Add information

            string beginning = "";
            beginning += "&title=" + this.information.GetValueOrDefault("Name") + "[宴]" + "\n";
            beginning += "&wholebpm=" + this.information.GetValueOrDefault("BPM") + "\n";
            beginning += "&artist=" + this.information.GetValueOrDefault("Composer") + "\n";
            beginning += "&des=" + this.information.GetValueOrDefault("Master Chart Maker") + "\n";
            beginning += "&shortid=" + this.information.GetValueOrDefault("Music ID") + "\n";
            beginning += "&genre=" + this.information.GetValueOrDefault("Genre") + "\n";
            beginning += "&cabinate=SD";
            beginning += "&version=" + this.musicXml.TrackVersion + "\n";
            beginning += "&chartconverter=Neskol\n";
            beginning += "\n";

            int defaultChartIndex = 7;
            if (ma2files.Count > 1)
            {
                defaultChartIndex = 0;
            }

            foreach (string ma2file in ma2files)
            {
                beginning += "&lv_" + defaultChartIndex + "=" + "宴" + "\n";
                beginning += "\n";
            }

            result += beginning;
            Console.WriteLine("Finished writing header of " + this.information.GetValueOrDefault("Name"));

            //Compose charts

            if (defaultChartIndex < 7)
            {
                for (int i = 0; i < this.charts.Count; i++)
                {
                    // Console.WriteLine("Processing chart: " + i);
                    if (!this.information[difficulty[i]].Equals(""))
                    {
                        string? isDxChart = "Utage";
                        result += "&inote_" + (i + 2) + "=\n";
                        result += this.Compose(charts[i]);
                        Program.CompiledChart.Add(this.information.GetValueOrDefault("Name") + isDxChart + " [" + difficulty[i] + "]");
                    }
                    result += "\n";
                }
            }
            else
            {
                result += "&inote_7=\n";
                result += this.Compose(charts[0]);
                Program.CompiledChart.Add(this.information.GetValueOrDefault("Name") + "Utage" + " [宴]");
            }

            Console.WriteLine("Finished composing.");
            return result;
        }

        public void TakeInformation(Dictionary<string, string> information)
        {
            this.information = information;
        }

        /// <summary>
        /// Return the chart bpm change table of MaiCompiler
        /// </summary>
        /// <returns>First BPM change table of this.charts</returns>
        public BPMChanges SymbolicBPMTable()
        {
            BPMChanges bpmTable = new BPMChanges();
            bool foundTable = false;
            for (int i = 0; i < this.charts.Count && !foundTable; i++)
            {
                if (this.charts[i] != null)
                {
                    bpmTable = this.charts[i].BPMChanges;
                    foundTable = true;
                }
            }
            return bpmTable;
        }

        /// <summary>
        /// Return the first note of master chart
        /// </summary>
        /// <returns>The first note of the master chart, or first note of the Utage chart if isUtage is turned true</returns>
        /// <exception cref="System.NullReferenceException">Throws null reference exception if the chart does not exist</exception>
        public Note SymbolicFirstNote(bool isUtage)
        {
            if (!isUtage && this.charts[3].CheckValidity())
            {
                return this.charts[3].FirstNote ?? throw new NullReferenceException("Null first note: master chart is invalid");
            }
            else if(isUtage)
            {
                return this.charts[0].FirstNote ?? throw new NullReferenceException("Null first note: utage chart is invalid");
            }
            else throw new NullReferenceException("This compiler contains invalid Master Chart and is not Utage Chart: no first note is returned");
        }

        /// <summary>
        /// Generate one line summary of this track with ID, name, genere and difficulty
        /// </summary>
        /// <returns></returns>
        public string GenerateOneLineSummary()
        {
            string result = "";
            if (this.charts.Equals(null) || !this.CheckValidity())
            {
                throw new NullReferenceException("This compiler has empty chat list!");
            }
            result += "(" + this.information["Music ID"] + ")"+this.information["Name"]+", "+this.information["Genre"]+", ";
            
            return result;
        }
    }
}
