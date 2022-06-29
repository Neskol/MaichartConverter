using System.Dynamic;
using System.Globalization;

namespace MaichartConverter
{
    public class BPMChanges
    {
        private HashSet<BPMChange> changeNotes;

        /// <summary>
        /// Construct with changes listed
        /// </summary>
        /// <param name="bar">Bar which contains changes</param>
        /// <param name="tick">Tick in bar contains changes</param>
        /// <param name="bpm">Specified BPM changes</param>
        public BPMChanges(List<int> bar, List<int> tick, List<double> bpm)
        {
            this.changeNotes = new HashSet<BPMChange>();
            for (int i = 0; i < bar.Count; i++)
            {
                BPMChange candidate = new(bar[i], tick[i], bpm[i]);
                changeNotes.Add(candidate);
            }
            this.Update();
        }

        /// <summary>
        /// Construct empty BPMChange List
        /// </summary>
        public BPMChanges()
        {
            this.changeNotes = new HashSet<BPMChange>();
            this.Update();
        }

        /// <summary>
        /// Construct BPMChanges with existing one
        /// </summary>
        /// <param name="takenIn"></param>
        public BPMChanges(BPMChanges takenIn)
        {
            this.changeNotes = new HashSet<BPMChange>();
            foreach (BPMChange candidate in takenIn.ChangeNotes)
            {
                this.changeNotes.Add(candidate);
            }
        }

        public List<BPMChange> ChangeNotes
        {
            get
            {
                List<BPMChange> result = new();
                result.AddRange(this.changeNotes);
                return result;
            }
        }

        /// <summary>
        /// Add BPMChange to change notes
        /// </summary>
        /// <param name="takeIn"></param>
        public void Add(BPMChange takeIn)
        {
            this.changeNotes.Add(takeIn);
            this.Update();
        }

        /// <summary>
        /// Compose change notes according to BPMChanges
        /// </summary>
        public void Update()
        {
            List<BPMChange> adjusted = new();
            Note lastNote = new Rest();
            foreach (BPMChange x in this.changeNotes)
            {
                if (!(x.Bar == lastNote.Bar && x.Tick == lastNote.Tick && x.BPM == lastNote.BPM))
                {
                    adjusted.Add(x);
                    lastNote = x;
                }
            }
            // Console.WriteLine(adjusted.Count);
            this.changeNotes = new HashSet<BPMChange>();
            foreach(BPMChange x in adjusted)
            {
                this.changeNotes.Add(x);
            }
            if (this.changeNotes.Count!=adjusted.Count)
            {
                throw new Exception("Adjusted BPM Note number not matching");
            }
        }

        /// <summary>
        /// Returns first definitions
        /// </summary>
        public string InitialChange
        {
            get
            {
                if (changeNotes.Count > 4)
                {
                    string result = "BPM_DEF" + "\t";
                    for (int x = 0; x < 4; x++)
                    {
                        result = result + String.Format("{0:F3}", this.ChangeNotes[x].BPM);
                        result += "\t";
                    }
                    return result + "\n";
                }
                else
                {
                    int times = 0;
                    string result = "BPM_DEF" + "\t";
                    foreach (BPMChange x in changeNotes)
                    {
                        result += String.Format("{0:F3}", x.BPM);
                        result += "\t";
                        times++;
                    }
                    while (times < 4)
                    {
                        result += String.Format("{0:F3}", this.ChangeNotes[0].BPM);
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
            bool result = true;
            return result;
        }

        /// <summary>
        /// Compose BPMChanges in beginning of MA2
        /// </summary>
        /// <returns></returns>
        public string Compose()
        {
            string result = "";
            for (int i = 0; i < changeNotes.Count; i++)
            {
                result += "BPM" + "\t" + this.ChangeNotes[i].Bar + "\t" + this.ChangeNotes[i].Tick + "\t" + this.ChangeNotes[i].BPM + "\n";
                //result += "BPM" + "\t" + bar[i] + "\t" + tick[i] + "\t" + String.Format("{0:F3}", bpm[i])+"\n";
            }
            return result;
        }
    }
}
