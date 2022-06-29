namespace MaichartConverter
{
    /// <summary>
    /// Store measure change notes in a chart
    /// </summary>
    public class MeasureChanges
    {
        private List<int> bar;
        private List<int> tick;
        private List<int> quavers;
        private List<int> beats;
        private List<MeasureChange> changeNotes;
        private int initialQuaver;
        private int initialBeat;

        /// <summary>
        /// Construct an empty Measure Change
        /// </summary>
        public MeasureChanges()
        {
            bar = new List<int>();
            tick = new List<int>();
            quavers = new List<int>();
            beats = new List<int>();
            changeNotes = new List<MeasureChange>();
        }

        /// <summary>
        /// Construct Measure Change with existing one
        /// </summary>
        public MeasureChanges(MeasureChanges takeIn)
        {
            bar = new List<int>(takeIn.Bar);
            tick = new List<int>(takeIn.Tick);
            quavers = new List<int>(takeIn.Quavers);
            beats = new List<int>(takeIn.Beats);
            changeNotes = new List<MeasureChange>(takeIn.ChangeNotes);
        }

        /// <summary>
        /// Take in initial quavers and beats, incase MET_CHANGE is not specified
        /// </summary>
        /// <param name="initialQuaver">Initial Quaver</param>
        /// <param name="initialBeat">Initial Beat</param>
        public MeasureChanges(int initialQuaver, int initialBeat)
        {
            bar = new List<int>();
            tick = new List<int>();
            quavers = new List<int>();
            beats = new List<int>();
            changeNotes = new List<MeasureChange>();
            this.initialQuaver = initialQuaver;
            this.initialBeat = initialBeat;
        }

        /// <summary>
        /// Construct a measure of given beats
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <param name="quavers"></param>
        /// <param name="beats"></param>
        public MeasureChanges(List<int> bar, List<int> tick, List<int> quavers, List<int> beats)
        {
            this.bar = bar;
            this.tick = tick;
            this.quavers = quavers;
            this.initialQuaver = quavers[0];
            this.beats = beats;
            this.initialBeat = beats[0];
            changeNotes = new List<MeasureChange>();
        }

        /// <summary>
        /// Return this.Bar
        /// </summary>
        public List<int> Bar
        {
            get { return bar; }
        }

        /// <summary>
        /// Return this.Tick
        /// </summary>
        public List<int> Tick
        {
            get { return tick; }
        }

        /// <summary>
        /// Return this.Quavers
        /// </summary>
        public List<int> Quavers
        {
            get { return quavers; }
        }

        /// <summary>
        /// Return this.Beats
        /// </summary>
        public List<int> Beats
        {
            get { return beats; }
        }

        public List<MeasureChange> ChangeNotes
        {
            get { return changeNotes; }
        }

        

        /// <summary>
        /// Add new measure changes to MeasureChanges
        /// </summary>
        /// <param name="bar">Bar which changes</param>
        /// <param name="tick">Tick which changes</param>
        /// <param name="quavers">Quavers which changes</param>
        /// <param name="beats">Beat which changes</param>
        public void Add(int bar, int tick, int quavers, int beats)
        {
            this.bar.Add(bar);
            this.tick.Add(tick);
            this.quavers.Add(quavers);
            this.beats.Add(beats);
        }

        /// <summary>
        /// Return first definitions
        /// </summary>
        public string InitialChange
        {
            get
            {
                return "MET_DEF" + "\t" + this.initialQuaver + "\t" + this.initialBeat + "\n";
            }
        }

        public bool CheckValidity()
        {
            bool result = bar.IndexOf(0) == 0;
            result = result && tick.IndexOf(0) == 0;
            result = result && !quavers[0].Equals(null);
            result = result && !beats[0].Equals(null);
            return result;
        }

        public string Compose()
        {
            string result = "";
            if (bar.Count == 0)
            {
                result += "MET" + "\t" + 0 + "\t" + 0 + "\t" + 4 + "\t" + 4 + "\n";
            }
            else
            {
                for (int i = 0; i < bar.Count; i++)
                {
                    result += "MET" + "\t" + bar[i] + "\t" + tick[i] + "\t" + quavers[i] + "\t" + beats[i] + "\n";
                }
            }
            return result;
        }

        public void Update()
        {
        }
    }
}
