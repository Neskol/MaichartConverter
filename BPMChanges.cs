namespace MaichartConverter
{
    public class BPMChanges : IChart
    {
        private List<int> bar;
        private List<int> tick;
        private List<double> bpm;
        private List<BPMChange> changeNotes;

        /// <summary>
        /// Construct with changes listed
        /// </summary>
        /// <param name="bar">Bar which contains changes</param>
        /// <param name="tick">Tick in bar contains changes</param>
        /// <param name="bpm">Specified BPM changes</param>
        public BPMChanges(List<int> bar, List<int> tick, List<double> bpm)
        {
            this.bar = bar;
            this.tick = tick;
            this.bpm = bpm;
            this.changeNotes = new List<BPMChange>();
            this.Update();
        }

        /// <summary>
        /// Construct empty BPMChange List
        /// </summary>
        public BPMChanges()
        {
            this.bar = new List<int>();
            this.tick = new List<int>();
            this.bpm = new List<double>();
            this.changeNotes = new List<BPMChange>();
            this.Update();
        }

        /// <summary>
        /// Return this.Bar
        /// </summary>
        public List<int> Bar
        {
            get { return this.bar; }
        }

        /// <summary>
        /// Return this.Tick
        /// </summary>
        public List<int> Tick
        {
            get { return this.tick; }
        }

        /// <summary>
        /// Return this.bpm
        /// </summary>
        public List<double> Bpm
        {
            get { return this.bpm; }
        }

        public List<BPMChange> ChangeNotes
        {
            get
            {
                return this.changeNotes;
            }
        }

        /// <summary>
        /// Add new BPM Changes to BPM Changes
        /// </summary>
        /// <param name="bar">Bar to change</param>
        /// <param name="tick">Tick to change</param>
        /// <param name="bpm">BPM to change</param>
        public void Add(int bar, int tick, double bpm)
        {
            this.bar.Add(bar);
            this.tick.Add(tick);
            this.bpm.Add(bpm);
            BPMChange x = new BPMChange(bar, tick, bpm);
            this.changeNotes.Add(x);
        }

        /// <summary>
        /// Compose change notes according to BPMChanges
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < tick.Count; i++)
            {
                this.changeNotes.Add(new BPMChange(bar[i], tick[i], bpm[i]));
            }
        }

        /// <summary>
        /// Returns first definitions
        /// </summary>
        public string InitialChange
        {
            get
            {
                if (bar.Count > 4)
                {
                    string result = "BPM_DEF" + "\t";
                    for (int x = 0; x < 4; x++)
                    {
                        result = result + String.Format("{0:F3}", bpm[x]);
                        result += "\t";
                    }
                    return result + "\n";
                }
                else
                {
                    int times = 0;
                    string result = "BPM_DEF" + "\t";
                    foreach (double x in bpm)
                    {
                        result += String.Format("{0:F3}", x);
                        result += "\t";
                        times++;
                    }
                    while (times < 4)
                    {
                        result += String.Format("{0:F3}", bpm[0]);
                        result += "\t";
                        times++;
                    }
                    return result + "\n";
                }
            }
        }

        /// <summary>
        /// See if the BPMChange is valid
        /// </summary>
        /// <returns>True if valid, false elsewise</returns>
        public bool CheckValidity()
        {
            bool result = bar.IndexOf(0) == 0;
            result = result && tick.IndexOf(0) == 0;
            result = result && !bpm[0].Equals(null);
            return result;
        }

        /// <summary>
        /// Compose BPMChanges in beginning of MA2
        /// </summary>
        /// <returns></returns>
        public string Compose()
        {
            string result = "";
            for (int i = 0; i < bar.Count; i++)
            {
                result += "BPM" + "\t" + changeNotes[i].Bar + "\t" + changeNotes[i].StartTime + "\t" + changeNotes[i].BPM + "\n";
                //result += "BPM" + "\t" + bar[i] + "\t" + tick[i] + "\t" + String.Format("{0:F3}", bpm[i])+"\n";
            }
            return result;
        }
    }
}
