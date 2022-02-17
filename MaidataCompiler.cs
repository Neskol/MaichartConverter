using System.Xml;

namespace MaichartConverter
{
    internal class MaidataCompiler : ICompiler
    {
        public static readonly string[] difficulty = { "Basic", "Advanced", "Expert", "Master", "Remaster" };
        private List<Chart> charts;
        private Dictionary<string, string> information;
        private XmlInformation musicXml;

        /// <summary>
        /// Construct compiler of a single song.
        /// </summary>
        /// <param name="location">Folder</param>
        /// <param name="targetLocation">Output folder</param>
        public MaidataCompiler(string location, string targetLocation)
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
            StreamWriter sw = new StreamWriter(targetLocation + Program.sep + "maidata.txt", false);
            {
                sw.WriteLine(result);
            }
            sw.Close();
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public MaidataCompiler()
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
                beginning += "&title=" + this.information.GetValueOrDefault("Name") + "\n";
                beginning += "&wholebpm=" + this.information.GetValueOrDefault("BPM") + "\n";
                beginning += "&artist=" + this.information.GetValueOrDefault("Composer") + "\n";
                beginning += "&des=" + this.information.GetValueOrDefault("Master Chart Maker")+"\n";
                beginning += "&shortid=" + this.information.GetValueOrDefault("Music ID") + "\n";
                beginning += "&genre=" + this.information.GetValueOrDefault("Genre") + "\n";
                beginning += "&cabinate=";
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
                        result += "&inote_" + (i + 2) + "=\n";
                        result += this.Compose(charts[i]);
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
            //    Console.WriteLine("BPM Change verified in " + x.Bar + " " + x.StartTime + " of BPM" + x.BPM);
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
                            if (x.IsNote && ((!x.NoteSpecificType.Equals("SLIDE")) && x.StartTime == lastNote.StartTime && !x.NoteGenre.Equals("BPM")))
                            {
                                result += "/";
                            }
                            else result += ",";
                            break;
                        case "HOLD":
                            if (x.IsNote && (!x.NoteSpecificType.Equals("SLIDE")) && x.StartTime == lastNote.StartTime && !x.NoteGenre.Equals("BPM"))
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
                            if (x.IsNote && (!x.NoteSpecificType.Equals("SLIDE")) && x.StartTime == lastNote.StartTime && !x.NoteGenre.Equals("BPM"))
                            {
                                result += "/";
                            }
                            else if (x.IsNote && x.NoteSpecificType.Equals("SLIDE") && x.StartTime == lastNote.StartTime && !x.NoteGenre.Equals("BPM"))
                            {
                                result += "*";
                            }
                            else result += ",";
                            break;
                        default:
                            result += ",";
                            break;
                    }
                    result += x.Compose(0);
                    lastNote = x;
                    //if (x.NoteGenre().Equals("BPM"))
                    //{
                    //    result+="("+ x.Bar + "_" + x.StartTime + ")";
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

        public void TakeInformation(Dictionary<string, string> information)
        {
            this.information = information;
        }
    }
}
