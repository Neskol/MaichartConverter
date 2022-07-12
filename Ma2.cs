namespace MaichartConverter
{
    /// <summary>
    /// Implementation of chart in ma2 format.
    /// </summary>
    public class Ma2 : Chart, ICompiler
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Ma2()
        {
            this.Notes = new List<Note>();
            this.BPMChanges = new BPMChanges();
            this.MeasureChanges = new MeasureChanges();
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
        }

        /// <summary>
        /// Construct Ma2 with given notes, bpm change definitions and measure change definitions.
        /// </summary>
        /// <param name="notes">Notes in Ma2</param>
        /// <param name="bpmChanges">BPM Changes: Initial BPM is NEEDED!</param>
        /// <param name="measureChanges">Measure Changes: Initial Measure is NEEDED!</param>
        public Ma2(List<Note> notes, BPMChanges bpmChanges, MeasureChanges measureChanges)
        {
            this.Notes = new List<Note>(notes);
            this.BPMChanges = new BPMChanges(bpmChanges);
            this.MeasureChanges = new MeasureChanges(measureChanges);
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother from location specified
        /// </summary>
        /// <param name="location">MA2 location</param>
        public Ma2(string location)
        {
            string[] tokens = new Ma2Tokenizer().Tokens(location);
            Chart takenIn = new Ma2Parser().ChartOfToken(tokens);
            this.Notes = new List<Note>(takenIn.Notes);
            this.BPMChanges = new BPMChanges(takenIn.BPMChanges);
            this.MeasureChanges = new MeasureChanges(takenIn.MeasureChanges);
            this.StoredChart = new List<List<Note>>(takenIn.StoredChart);
            this.Information = new Dictionary<string, string>(takenIn.Information);
            this.Update();
        }

        /// <summary>
        /// Construct Ma2 with tokens given
        /// </summary>
        /// <param name="tokens">Tokens given</param>
        public Ma2(string[] tokens)
        {
            Chart takenIn = new Ma2Parser().ChartOfToken(tokens);
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>(takenIn.StoredChart);
            this.Information = new Dictionary<string, string>(takenIn.Information);
            this.Update();
        }

        /// <summary>
        /// Construct Ma2 with existing values
        /// </summary>
        /// <param name="takenIn">Existing good brother</param>
        public Ma2(Chart takenIn)
        {
            this.Notes = new List<Note>(takenIn.Notes);
            this.BPMChanges = new BPMChanges(takenIn.BPMChanges);
            this.MeasureChanges = new MeasureChanges(takenIn.MeasureChanges);
            this.StoredChart = new List<List<Note>>(takenIn.StoredChart);
            this.Information = new Dictionary<string, string>(takenIn.Information);
            this.Update();
        }

        public override string Compose()
        {
            string result = "";
            const string header1 = "VERSION\t0.00.00\t1.03.00\nFES_MODE\t0\n";
            const string header2 = "RESOLUTION\t384\nCLK_DEF\t384\nCOMPATIBLE_CODE\tMA2\n";
            result += header1;
            result += BPMChanges.InitialChange;
            result += MeasureChanges.InitialChange;
            result += header2;
            result += "\n";

            result += BPMChanges.Compose();
            result += MeasureChanges.Compose();
            result += "\n";

            //foreach (Note x in this.Notes)
            //{
            //    if (!x.Compose(1).Equals(""))
            //    {
            //        result += x.Compose(1) + "\n";
            //    }
            //}
            foreach (List<Note> bar in this.StoredChart)
            {
                foreach (Note x in bar)
                {
                    if (!x.Compose(1).Equals(""))
                    {
                        result += x.Compose(1) + "\n";
                    }
                }
            }
            result += "\n";
            return result;
        }

        /// <summary>
        /// Override and compose with given arrays
        /// </summary>
        /// <param name="bpm">Override BPM array</param>
        /// <param name="measure">Override Measure array</param>
        /// <returns>Good Brother with override array</returns>
        public override string Compose(BPMChanges bpm, MeasureChanges measure)
        {
            string result = "";
            const string header1 = "VERSION\t0.00.00\t1.03.00\nFES_MODE\t0\n";
            const string header2 = "RESOLUTION\t384\nCLK_DEF\t384\nCOMPATIBLE_CODE\tMA2\n";
            result += header1;
            result += bpm.InitialChange;
            result += measure.InitialChange;
            result += header2;
            result += "\n";

            result += bpm.Compose();
            result += measure.Compose();
            result += "\n";

            foreach (Note y in this.Notes)
            {
                result += y.Compose(1) + "\n";
            }
            result += "\n";
            return result;
        }
    }
}
