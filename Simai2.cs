namespace MaichartConverter
{
    public class Simai2 : Chart
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Simai2()
        {
            this.Notes = new List<Note>();
            this.BPMChanges = new BPMChanges();
            this.MeasureChanges = new MeasureChanges();
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
        }

        /// <summary>
        /// Construct Simai from given parameters
        /// </summary>
        /// <param name="notes">Notes to take in</param>
        /// <param name="bpmChanges">BPM change to take in</param>
        /// <param name="measureChanges">Measure change to take in</param>
        public Simai2(List<Note> notes, BPMChanges bpmChanges, MeasureChanges measureChanges)
        {
            this.Notes = notes;
            this.BPMChanges = bpmChanges;
            this.MeasureChanges = measureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.Update();
        }

        public Simai2(Simai2 takenIn)
        {
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.Update();
        }

        public override string Compose()
        {
            string result = "";
            int delayBar = (this.TotalDelay) / 384 + 2;
            //Console.WriteLine(chart.Compose());
            //foreach (BPMChange x in chart.BPMChanges.ChangeNotes)
            //{
            //    Console.WriteLine("BPM Change verified in " + x.Bar + " " + x.Tick + " of BPM" + x.BPM);
            //}
            List<Note> firstBpm = new List<Note>();
            foreach (Note bpm in this.Notes)
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
            foreach (List<Note> bar in this.StoredChart)
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
        /// Reconstruct the chart with given arrays
        /// </summary>
        /// <param name="bpm">New BPM Changes</param>
        /// <param name="measure">New Measure Changes</param>
        /// <returns>New Composed Chart</returns>
        public override string Compose(BPMChanges bpm, MeasureChanges measure)
        {
            BPMChanges sourceBPM = this.BPMChanges;
            MeasureChanges sourceMeasures = this.MeasureChanges;
            this.BPMChanges=bpm;
            this.MeasureChanges=measure;
            this.Update();

            string result = this.Compose();
            this.BPMChanges=sourceBPM;
            this.MeasureChanges=sourceMeasures;
            this.Update();
            return result;
        }
    }
}