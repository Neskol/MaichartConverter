namespace MaichartConverter
{
    /// <summary>
    /// Good Brother Implementation.
    /// </summary>
    public class Ma2 : Chart, ICompiler
    {

        public Ma2()
        {
            this.Notes = new List<Note>();
            this.BPMChanges = new BPMChanges();
            this.MeasureChanges = new MeasureChanges();
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
        }

        /// <summary>
        /// Construct Good Brother with given notes, bpm change definitions and measure change definitions.
        /// </summary>
        /// <param name="notes">Notes in Good Brother</param>
        /// <param name="bpmChanges">BPM Changes: Initial BPM is NEEDED!</param>
        /// <param name="measureChanges">Measure Changes: Initial Measure is NEEDED!</param>
        public Ma2(List<Note> notes, BPMChanges bpmChanges, MeasureChanges measureChanges)
        {
            this.Notes = notes;
            this.BPMChanges = bpmChanges;
            this.MeasureChanges = measureChanges;
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
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother with tokens given
        /// </summary>
        /// <param name="tokens">Tokens given</param>
        public Ma2(string[] tokens)
        {
            Chart takenIn = new Ma2Parser().ChartOfToken(tokens);
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother with existing values
        /// </summary>
        /// <param name="takenIn">Existing good brother</param>
        public Ma2(Ma2 takenIn)
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

            foreach (Note x in this.Notes)
            {
                result += x.Compose(1) + "\n";
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
