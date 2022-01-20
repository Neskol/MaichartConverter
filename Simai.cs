namespace MaidataConverter
{
    public class Simai : IGoodBrother, ICompiler
    {
        private string title;
        private List<double> difficulty;
        private List<string> noteDesigner;
        private double wholeBPM;
        private List<GoodBrother1> maps;
        public const int Resolution = 384; //Maximum tick of a bar;
        public const string BreakNote = "b", HoldNote = "h", SI_Note = "-", SV_Note = "v", SC_Note = "<>", SL_Note = "V", SX_Note = "p";
        public const string TouchPrefix = "ABCDEF";
        public const string SlideType = "-^vV<>pqszw";
        public Simai(string maidata)
        {
            title = "";
            difficulty = new List<double>();
            noteDesigner = new List<string>();
            maps = new List<GoodBrother1>();
            wholeBPM = 0.00;
            string[] processing = maidata.Split('&');
            foreach (string x in processing)
            {
                if (x.Contains("title"))
                {
                    string[] rawTitle = x.Split('=');
                    if (rawTitle.Length == 3)
                    {
                        this.title = rawTitle[2];
                    }
                    else if (rawTitle.Length >= 3)
                    {
                        string realTitle = "";
                        for (int y = 2; y < rawTitle.Length; y++)
                        {
                            realTitle += rawTitle[y];
                        }
                        this.title = realTitle;
                    }
                    else this.title = "Empty";
                }
                else if (x.Contains("wholebpm"))
                {
                    if (x.Split('=').Length == 2)
                    {
                        this.wholeBPM = Double.Parse(x.Split('=')[1]);
                    }
                    else this.wholeBPM = 0.00;
                }
                else if (x.Contains("inotes_"))
                {
                    if (x.Contains("inotes_1"))
                    {
                        this.maps.Add(Read(x, 1));
                    }
                    else if (x.Contains("inotes_2"))
                    {
                        this.maps.Add(Read(x, 2));
                    }
                    else if (x.Contains("inotes_3"))
                    {
                        this.maps.Add(Read(x, 3));
                    }
                    else if (x.Contains("inotes_4"))
                    {
                        this.maps.Add(Read(x, 4));
                    }
                    else if (x.Contains("inotes_5"))
                    {
                        this.maps.Add(Read(x, 5));
                    }
                }
            }
        }
        /// <summary>
        /// Takes in maidata section and compose into notes
        /// </summary>
        /// <param name="maiNotes">string to take in</param>
        /// <param name="Difficulty">difficulty to mark</param>
        /// <returns></returns>
        public GoodBrother1 Read(string maiNotes, int Difficulty)
        {
            List<Note> notes = new List<Note>();
            List<int> changedBars = new List<int>(), changedTicks = new List<int>(), changedQuavers = new List<int>(), changedBeats = new List<int>();
            List<double> changedBPMs = new List<double>();
            int bar = 1, tick = 0, quavers = 0;
            double bpm = this.wholeBPM;

            string[] processing = maiNotes.Split(')');
            string candidate = maiNotes;
            if (processing.Length >= 2)
            {
                foreach (string x in processing)
                {
                    candidate = String.Join(',', processing);
                }
            }
            processing = candidate.Split(',');
            for (int x = 0; x < processing.Length; x++)
            {
                if (processing[x].Contains("("))
                {
                    string[] y = processing[x].Split(')');
                    double changedBPM = Double.Parse(y[0].Split('(')[1]);
                    changedBPMs.Add(changedBPM);
                }
                else if (processing[x].Contains("{"))
                {
                    string[] y = processing[x].Split('}');
                    quavers = int.Parse(y[0].Split('}')[1]);
                }
                else notes.AddRange(Read(processing[x], bar, tick));
                tick += Resolution / quavers;
                if (tick >= Resolution)
                {
                    bar++;
                    tick = 0;
                }
            }
            BPMChanges bpmChanges = new BPMChanges(changedBars, changedTicks, changedBPMs);
            MeasureChanges measureChanges = new MeasureChanges(changedBars, changedTicks, changedQuavers, changedBeats);
            GoodBrother1 result = new GoodBrother1(notes, bpmChanges, measureChanges);
            return result;
        }

        public static List<Note> Read(string section, int bar, int tick)
        {
            List<Note> result = new List<Note>();
            if (section.Contains("/"))
            {
                string[] deeperSection = section.Split('/');
                foreach (string x in deeperSection)
                {
                    result.AddRange(Read(x, bar, tick));
                }
            }
            else if (section.Contains("*"))
            {
                string[] deeperSection = section.Split('*');
                for (int x = 1; x < deeperSection.Length; x++)
                {
                    deeperSection[x] = section.ToCharArray()[0] + deeperSection[x];
                }//Generate Slide Start for Each-Slides.
                foreach (string x in deeperSection)
                {
                    result.AddRange(Read(x, bar, tick));
                }
            }
            else if (int.TryParse(section, out int each))
            {
                char[] eachGroup = each.ToString().ToCharArray();
                if (eachGroup.Length >= 2)
                {
                    foreach (char x in eachGroup)
                    {
                        result.Add(new Tap("TAP", bar, tick, x.ToString()));
                    }
                }
            }
            else if (section.Length == 3 && section.Contains(BreakNote))
            {
                result.Add(new Tap("BRK", bar, tick, section.Remove(section.Length - 1)));
            }
            else if (section.Length == 2 && section.IndexOfAny(TouchPrefix.ToCharArray()) == 0)
            {
                result.Add(new Tap("TTP", bar, tick, section.Substring(1)));
            }
            else if (section.Contains(HoldNote))
            {
                string[] holdCandidate = section.Split('h');
                string[] holdTime = holdCandidate[1].Substring(1).Remove(section.Length - 1).Split(':');
                int holdQuaver = int.Parse(holdTime[0]);
                int holdMultiple = int.Parse(holdTime[1]);
                if (holdCandidate[0].Equals("C"))
                {
                    result.Add(new Hold("THO", bar, tick, "C1", (Resolution / holdQuaver) * holdMultiple));
                }
                else if (holdCandidate[0].Contains("x"))
                {
                    char[] xIncluded = holdCandidate[0].ToCharArray();
                    string xExcluded = ""; //Real key exclude X for XHO mark
                    foreach (char x in xIncluded)
                    {
                        if (x.Equals('x'))
                        {
                            xExcluded += x;
                        }
                    }
                    result.Add(new Hold("XHD", bar, tick, xExcluded, (Resolution / holdQuaver) * holdMultiple));
                }
                else result.Add(new Hold("HLD", bar, tick, holdCandidate[0], (Resolution / holdQuaver) * holdMultiple));
            }
            else if (section.IndexOfAny(SlideType.ToCharArray()) >= 0)
            {
                string[] typeCandidate = section.Split('[');
                string timeCandidate = typeCandidate[1].Substring(0, typeCandidate[1].Length - 1);
                string[] quaverCandidate = timeCandidate.Split(':');
                int overrideQuaver = Resolution / int.Parse(quaverCandidate[0]);
#pragma warning disable CS0219 // 变量“last”已被赋值，但从未使用过它的值
                int last = 0;
#pragma warning restore CS0219 // 变量“last”已被赋值，但从未使用过它的值
                if (section.Contains("-"))
                {

                }
                else if (section.Contains("^"))
                {

                }
                else if (section.Contains("v") || section.Contains("V"))
                {

                }
                else if (section.Contains("<") || section.Contains(">"))
                {

                }
                else if (section.Contains("p") || section.Contains("q"))
                {

                }
                else if (section.Contains("s") || section.Contains("z"))
                {

                }
                else if (section.Contains("w"))
                {

                }

            }
            return result;
        }
        public bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public string Compose()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void TakeInformation(Dictionary<string, string> information)
        {
            throw new NotImplementedException();
        }
    }
}
