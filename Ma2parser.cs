using System.Runtime.CompilerServices;

namespace MaichartConverter
{
    /// <summary>
    /// Parses ma2 file into Ma2 chart format
    /// </summary>
    public class Ma2Parser : IParser
    {
        private Tap PreviousSlideStart;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Ma2Parser()
        {
            PreviousSlideStart = new Tap();
        }

        public Chart ChartOfToken(string[] token)
        {
            BPMChanges bpmChanges = new BPMChanges();
            MeasureChanges measureChanges = new MeasureChanges();
            List<Note> notes = new List<Note>();
            if (token != null)
            {
                foreach (string x in token)
                {
                    bool isBPM_DEF = x.Split('\t')[0].Equals("BPM_DEF");
                    bool isMET_DEF = x.Split('\t')[0].Equals("MET_DEF");
                    bool isBPM = x.Split('\t')[0].Equals("BPM");
                    bool isMET = x.Split('\t')[0].Equals("MET");
                    bool isNOTE = x.Split('\t')[0].Equals("TAP")
                        || x.Split('\t')[0].Equals("STR")
                        || x.Split('\t')[0].Equals("TTP")
                        || x.Split('\t')[0].Equals("XTP")
                        || x.Split('\t')[0].Equals("XST")
                        || x.Split('\t')[0].Equals("BRK")
                        || x.Split('\t')[0].Equals("BST")
                        || x.Split('\t')[0].Equals("HLD")
                        || x.Split('\t')[0].Equals("XHO")
                        || x.Split('\t')[0].Equals("THO")
                        || x.Split('\t')[0].Equals("SI_")
                        || x.Split('\t')[0].Equals("SV_")
                        || x.Split('\t')[0].Equals("SF_")
                        || x.Split('\t')[0].Equals("SCL")
                        || x.Split('\t')[0].Equals("SCR")
                        || x.Split('\t')[0].Equals("SUL")
                        || x.Split('\t')[0].Equals("SUR")
                        || x.Split('\t')[0].Equals("SLL")
                        || x.Split('\t')[0].Equals("SLR")
                        || x.Split('\t')[0].Equals("SXL")
                        || x.Split('\t')[0].Equals("SXR")
                        || x.Split('\t')[0].Equals("SSL")
                        || x.Split('\t')[0].Equals("SSR");

                    if (isBPM_DEF)
                    {
                        bpmChanges = BPMChangesOfToken(x);
                    }
                    else if (isMET_DEF)
                    {
                        measureChanges = MeasureChangesOfToken(x);
                    }
                    else if (isBPM)
                    {
                        string[] bpmCandidate = x.Split('\t');
                        BPMChange candidate = new BPMChange(Int32.Parse(bpmCandidate[1]),
                            Int32.Parse(bpmCandidate[2]),
                            Double.Parse(bpmCandidate[3]));
                        // foreach (BPMChange change in bpmChanges.ChangeNotes)
                        // {
                        //     if (change.TickStamp <= candidate.LastTickStamp)
                        //     {
                        //         candidate.BPMChangeNotes.Add(change);
                        //         Console.WriteLine("A BPM change note was added with overall tick of "+change.TickStamp + " with bpm of "+change.BPM);
                        //     }
                        // }
                        bpmChanges.Add(candidate);
                        bpmChanges.Update();        
                    }
                    else if (isMET)
                    {
                        string[] measureCandidate = x.Split('\t');
                        measureChanges.Add(Int32.Parse(measureCandidate[1]),
                            Int32.Parse(measureCandidate[2]),
                            Int32.Parse(measureCandidate[3]),
                            Int32.Parse(measureCandidate[4]));
                    }
                    else if (isNOTE)
                    {
                        Note candidate = NoteOfToken(x);
                        // foreach (BPMChange change in bpmChanges.ChangeNotes)
                        // {
                        //     if (change.TickStamp <= candidate.LastTickStamp)
                        //     {
                        //         candidate.BPMChangeNotes.Add(change);
                        //         Console.WriteLine("A BPM change note was added with overall tick of " + change.TickStamp + " with bpm of " + change.BPM);
                        //     }
                        // }
                        notes.Add(candidate);
                    }
                }
            }
            foreach (Note note in notes)
            {
                note.BPMChangeNotes = bpmChanges.ChangeNotes;
                if (bpmChanges.ChangeNotes.Count>0 && note.BPMChangeNotes.Count == 0)
                {
                    throw new IndexOutOfRangeException("BPM COUNT DISAGREE");
                }
                if (bpmChanges.ChangeNotes.Count == 0)
                {
                    throw new IndexOutOfRangeException("BPM CHANGE COUNT DISAGREE");
                }
            }
            Chart result = new Ma2(notes, bpmChanges, measureChanges);
            return result;
        }

        public BPMChanges BPMChangesOfToken(string token)
        {
            return new BPMChanges();
        }

        public MeasureChanges MeasureChangesOfToken(string token)
        {
            return new MeasureChanges(Int32.Parse(token.Split('\t')[1]), Int32.Parse(token.Split('\t')[2]));
        }

        public Note NoteOfToken(string token)
        {
            Note result = new Rest();
            bool isTap = token.Split('\t')[0].Equals("TAP")
                || token.Split('\t')[0].Equals("STR")
                || token.Split('\t')[0].Equals("TTP")
                || token.Split('\t')[0].Equals("XTP")
                || token.Split('\t')[0].Equals("XST")
                || token.Split('\t')[0].Equals("BRK")
                || token.Split('\t')[0].Equals("BST");
            bool isHold = token.Split('\t')[0].Equals("HLD")
                || token.Split('\t')[0].Equals("XHO")
                || token.Split('\t')[0].Equals("THO");
            bool isSlide = token.Split('\t')[0].Equals("SI_")
                || token.Split('\t')[0].Equals("SV_")
                || token.Split('\t')[0].Equals("SF_")
                || token.Split('\t')[0].Equals("SCL")
                || token.Split('\t')[0].Equals("SCR")
                || token.Split('\t')[0].Equals("SUL")
                || token.Split('\t')[0].Equals("SUR")
                || token.Split('\t')[0].Equals("SLL")
                || token.Split('\t')[0].Equals("SLR")
                || token.Split('\t')[0].Equals("SXL")
                || token.Split('\t')[0].Equals("SXR")
                || token.Split('\t')[0].Equals("SSL")
                || token.Split('\t')[0].Equals("SSR"); ;
            string[] candidate = token.Split('\t');
            int bar = Int32.Parse(candidate[1]);
            int tick = Int32.Parse(candidate[2]);
            foreach (string x in candidate)
            {
                if (isTap)
                {
                    result = TapOfToken(token); 
                    if (result.NoteSpecificType.Equals("SLIDE_START"))
                    {
                        PreviousSlideStart = (Tap)result;
                    }
                }
                else if (isHold)
                {
                    result = HoldOfToken(token);
                }
                else if (isSlide)
                {
                    result = SlideOfToken(token);
                    result.SlideStart = PreviousSlideStart;
                }
            }
            if (result.Tick == 384)
            {
                result.Tick = 0;
                result.Bar++;
            }
            return result;
        }

        public Note NoteOfToken(string token, int bar, int tick, double bpm)
        {
            Note result = new Rest();
            bool isTap = token.Split('\t')[0].Equals("TAP")
                || token.Split('\t')[0].Equals("STR")
                || token.Split('\t')[0].Equals("TTP")
                || token.Split('\t')[0].Equals("XTP")
                || token.Split('\t')[0].Equals("XST")
                || token.Split('\t')[0].Equals("BRK")
                || token.Split('\t')[0].Equals("BST");
            bool isHold = token.Split('\t')[0].Equals("HLD")
                || token.Split('\t')[0].Equals("XHO")
                || token.Split('\t')[0].Equals("THO");
            bool isSlide = token.Split('\t')[0].Equals("SI_")
                || token.Split('\t')[0].Equals("SV_")
                || token.Split('\t')[0].Equals("SF_")
                || token.Split('\t')[0].Equals("SCL")
                || token.Split('\t')[0].Equals("SCR")
                || token.Split('\t')[0].Equals("SUL")
                || token.Split('\t')[0].Equals("SUR")
                || token.Split('\t')[0].Equals("SLL")
                || token.Split('\t')[0].Equals("SLR")
                || token.Split('\t')[0].Equals("SXL")
                || token.Split('\t')[0].Equals("SXR")
                || token.Split('\t')[0].Equals("SSL")
                || token.Split('\t')[0].Equals("SSR"); ;
            string[] candidate = token.Split('\t');
            foreach (string x in candidate)
            {
                if (isTap)
                {
                    result = TapOfToken(token, bar, tick, bpm);
                }
                else if (isHold)
                {
                    result = HoldOfToken(token, bar, tick, bpm);
                }
                else if (isSlide)
                {
                    result = SlideOfToken(token, bar, tick,PreviousSlideStart, bpm);
                }
            }
            if (result.Tick == 384)
            {
                result.Tick = 0;
                result.Bar++;
            }
            return result;
        }

        public Hold HoldOfToken(string token, int bar, int tick, double bpm)
        {
            Note result = new Rest();
            string[] candidate = token.Split('\t');
            if (candidate[0].Equals("THO") && candidate.Count() > 7)
            {
                result = new Hold(candidate[0],
                bar,
                tick,
                candidate[3] + candidate[5], Int32.Parse(candidate[4]),
                Int32.Parse(candidate[6]),
                candidate[7]); //candidate[6] is special effect
            }
            else if (candidate[0].Equals("THO") && candidate.Count() <= 7)
            {
                //Console.ReadLine();
                result = new Hold(candidate[0],
                Int32.Parse(candidate[1]),
                Int32.Parse(candidate[2]),
                candidate[3] + candidate[5], Int32.Parse(candidate[4]),
                Int32.Parse(candidate[6]),
                "M1"); //candidate[6] is special effect
            }
            else
                result = new Hold(candidate[0],
                            Int32.Parse(candidate[1]),
                            Int32.Parse(candidate[2]),
                            candidate[3],
                            Int32.Parse(candidate[4]));
            result.BPM = bpm;
            return (Hold)result;
        }

        public Hold HoldOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = Int32.Parse(candidate[1]);
            int tick = Int32.Parse(candidate[2]);
            if (candidate[0].Equals("THO") && candidate.Count() > 7)
            {
                return new Hold(candidate[0],
                bar,
                tick,
                candidate[3] + candidate[5], Int32.Parse(candidate[4]),
                Int32.Parse(candidate[6]),
                candidate[7]); //candidate[6] is special effect
            }
            else if (candidate[0].Equals("THO") && candidate.Count() <= 7)
            {
                //Console.ReadLine();
                return new Hold(candidate[0],
                Int32.Parse(candidate[1]),
                Int32.Parse(candidate[2]),
                candidate[3] + candidate[5], Int32.Parse(candidate[4]),
                Int32.Parse(candidate[6]),
                "M1"); //candidate[6] is special effect
            }
            else
                return new Hold(candidate[0],
                            Int32.Parse(candidate[1]),
                            Int32.Parse(candidate[2]),
                            candidate[3],
                            Int32.Parse(candidate[4]));
        }

        public Slide SlideOfToken(string token, int bar, int tick, Note slideStart, double bpm)
        {
            Note result;
            string[] candidate = token.Split('\t');
            result = new Slide(candidate[0],
                                   bar,
                                   tick,
                                   slideStart.Key,
                                   Int32.Parse(candidate[4]),
                                   Int32.Parse(candidate[5]),
                                   candidate[6]);
            if (!slideStart.Key.Equals(candidate[3]))
            {
                throw new Exception("THE SLIDE START DOES NOT MATCH WITH THE DEFINITION OF THIS NOTE!");
            }
            result.BPM = bpm;
            return (Slide)result;
        }

        public Slide SlideOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = Int32.Parse(candidate[1]);
            int tick = Int32.Parse(candidate[2]);
            if (!PreviousSlideStart.Key.Equals(candidate[3]))
            {
                Console.WriteLine("Expected key: "+candidate[3]);
                Console.WriteLine("Actual key: "+PreviousSlideStart.Key);
                Console.WriteLine("Previous Slide Start: "+ PreviousSlideStart.Compose(1));
                throw new Exception("THE SLIDE START DOES NOT MATCH WITH THE DEFINITION OF THIS NOTE!");
            }
            return new Slide(candidate[0],
                        bar,
                        tick,
                        PreviousSlideStart.Key,
                        Int32.Parse(candidate[4]),
                        Int32.Parse(candidate[5]),
                        candidate[6]);
        }


        public Tap TapOfToken(string token, int bar, int tick, double bpm)
        {
            Note result = new Rest();
            string[] candidate = token.Split('\t');
            if (candidate[0].Equals("TTP") && (candidate.Count()) >= 7)
            {
                result = new Tap(candidate[0],
                bar,
                tick,
                candidate[3] + candidate[4],
                Int32.Parse(candidate[5]),
                candidate[6]);
            }
            else if (candidate[0].Equals("TTP") && (candidate.Count()) < 7)
            {
                //Console.ReadLine();
                result = new Tap(candidate[0],
                Int32.Parse(candidate[1]),
                Int32.Parse(candidate[2]),
                candidate[3] + candidate[4],
                Int32.Parse(candidate[5]),
                "M1");
            }
            else
                result = new Tap(candidate[0],
                    Int32.Parse(candidate[1]),
                    Int32.Parse(candidate[2]),
                    candidate[3]);
            result.BPM = bpm;
            return (Tap)result;
        }

        public Tap TapOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = Int32.Parse(candidate[1]);
            int tick = Int32.Parse(candidate[2]);
            if (candidate[0].Equals("TTP") && (candidate.Count()) >= 7)
            {
                return new Tap(candidate[0],
                bar,
                tick,
                candidate[3] + candidate[4],
                Int32.Parse(candidate[5]),
                candidate[6]);
            }
            else if (candidate[0].Equals("TTP") && (candidate.Count()) < 7)
            {
                //Console.ReadLine();
                return new Tap(candidate[0],
                Int32.Parse(candidate[1]),
                Int32.Parse(candidate[2]),
                candidate[3] + candidate[4],
                Int32.Parse(candidate[5]),
                "M1");
            }
            else
                return new Tap(candidate[0],
                    Int32.Parse(candidate[1]),
                    Int32.Parse(candidate[2]),
                    candidate[3]);
        }
    }
}

