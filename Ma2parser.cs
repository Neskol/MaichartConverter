using System.Runtime.CompilerServices;

namespace MaichartConverter
{
    public enum StdParam { Type, Bar, Tick, KeyOrParam, WaitTimeOrParam, LastTime, EndKey };
    public enum DxParam { Type, Bar, Tick, Key, KeyGroupOrLastTime, SpecialEffect, NoteSize };
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
                    bool isBPM_DEF = x.Split('\t')[(int)StdParam.Type].Equals("BPM_DEF");
                    bool isMET_DEF = x.Split('\t')[(int)StdParam.Type].Equals("MET_DEF");
                    bool isBPM = x.Split('\t')[(int)StdParam.Type].Equals("BPM");
                    bool isMET = x.Split('\t')[(int)StdParam.Type].Equals("MET");
                    bool isNOTE = x.Split('\t')[(int)StdParam.Type].Equals("TAP")
                        || x.Split('\t')[(int)StdParam.Type].Equals("STR")
                        || x.Split('\t')[(int)StdParam.Type].Equals("TTP")
                        || x.Split('\t')[(int)StdParam.Type].Equals("XTP")
                        || x.Split('\t')[(int)StdParam.Type].Equals("XST")
                        || x.Split('\t')[(int)StdParam.Type].Equals("BRK")
                        || x.Split('\t')[(int)StdParam.Type].Equals("BST")
                        || x.Split('\t')[(int)StdParam.Type].Equals("HLD")
                        || x.Split('\t')[(int)StdParam.Type].Equals("XHO")
                        || x.Split('\t')[(int)StdParam.Type].Equals("THO")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SI_")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SV_")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SF_")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SCL")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SCR")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SUL")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SUR")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SLL")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SLR")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SXL")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SXR")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SSL")
                        || x.Split('\t')[(int)StdParam.Type].Equals("SSR");

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
                        measureChanges.Add(Int32.Parse(measureCandidate[(int)StdParam.Bar]),
                            Int32.Parse(measureCandidate[(int)StdParam.Tick]),
                            Int32.Parse(measureCandidate[(int)StdParam.KeyOrParam]),
                            Int32.Parse(measureCandidate[(int)StdParam.WaitTimeOrParam]));
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
            bool isTap = token.Split('\t')[(int)StdParam.Type].Equals("TAP")
                || token.Split('\t')[(int)StdParam.Type].Equals("STR")
                || token.Split('\t')[(int)StdParam.Type].Equals("TTP")
                || token.Split('\t')[(int)StdParam.Type].Equals("XTP")
                || token.Split('\t')[(int)StdParam.Type].Equals("XST")
                || token.Split('\t')[(int)StdParam.Type].Equals("BRK")
                || token.Split('\t')[(int)StdParam.Type].Equals("BST");
            bool isHold = token.Split('\t')[(int)StdParam.Type].Equals("HLD")
                || token.Split('\t')[(int)StdParam.Type].Equals("XHO")
                || token.Split('\t')[(int)StdParam.Type].Equals("THO");
            bool isSlide = token.Split('\t')[(int)StdParam.Type].Equals("SI_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SV_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SF_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SCL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SCR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SUL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SUR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SLL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SLR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SXL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SXR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SSL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SSR");
            string[] candidate = token.Split('\t');
            int bar = Int32.Parse(candidate[(int)StdParam.Bar]);
            int tick = Int32.Parse(candidate[(int)StdParam.Tick]);
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
            bool isTap = token.Split('\t')[(int)StdParam.Type].Equals("TAP")
                || token.Split('\t')[(int)StdParam.Type].Equals("STR")
                || token.Split('\t')[(int)StdParam.Type].Equals("TTP")
                || token.Split('\t')[(int)StdParam.Type].Equals("XTP")
                || token.Split('\t')[(int)StdParam.Type].Equals("XST")
                || token.Split('\t')[(int)StdParam.Type].Equals("BRK")
                || token.Split('\t')[(int)StdParam.Type].Equals("BST");
            bool isHold = token.Split('\t')[(int)StdParam.Type].Equals("HLD")
                || token.Split('\t')[(int)StdParam.Type].Equals("XHO")
                || token.Split('\t')[(int)StdParam.Type].Equals("THO");
            bool isSlide = token.Split('\t')[(int)StdParam.Type].Equals("SI_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SV_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SF_")
                || token.Split('\t')[(int)StdParam.Type].Equals("SCL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SCR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SUL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SUR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SLL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SLR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SXL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SXR")
                || token.Split('\t')[(int)StdParam.Type].Equals("SSL")
                || token.Split('\t')[(int)StdParam.Type].Equals("SSR");
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
            if (candidate[(int)StdParam.Type].Equals("THO") && candidate.Count() > 7)
            {
                result = new Hold(candidate[(int)StdParam.Type],
                bar,
                tick,
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.LastTime], int.Parse(candidate[(int)DxParam.SpecialEffect]),
                int.Parse(candidate[(int)StdParam.EndKey]),
                candidate[7]); //candidate[(int)StdParam.EndKey] is special effect
            }
            else if (candidate[(int)StdParam.Type].Equals("THO") && candidate.Count() <= 7)
            {
                //Console.ReadLine();
                result = new Hold(candidate[(int)StdParam.Type],
                int.Parse(candidate[(int)StdParam.Bar]),
                int.Parse(candidate[(int)StdParam.Tick]),
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.LastTime], int.Parse(candidate[(int)StdParam.WaitTimeOrParam]),
                int.Parse(candidate[(int)StdParam.EndKey]),
                "M1"); //candidate[(int)StdParam.EndKey] is special effect
            }
            else
                result = new Hold(candidate[(int)StdParam.Type],
                            int.Parse(candidate[(int)StdParam.Bar]),
                            int.Parse(candidate[(int)StdParam.Tick]),
                            candidate[(int)StdParam.KeyOrParam],
                            int.Parse(candidate[(int)StdParam.WaitTimeOrParam]));
            result.BPM = bpm;
            return (Hold)result;
        }

        public Hold HoldOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = int.Parse(candidate[(int)StdParam.Bar]);
            int tick = int.Parse(candidate[(int)StdParam.Tick]);
            if (candidate[(int)StdParam.Type].Equals("THO") && candidate.Count() > 7)
            {
                return new Hold(candidate[(int)StdParam.Type],
                bar,
                tick,
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.LastTime], int.Parse(candidate[(int)StdParam.WaitTimeOrParam]),
                int.Parse(candidate[(int)StdParam.EndKey]),
                candidate[7]); //candidate[(int)StdParam.EndKey] is special effect
            }
            else if (candidate[(int)StdParam.Type].Equals("THO") && candidate.Count() <= 7)
            {
                //Console.ReadLine();
                return new Hold(candidate[(int)StdParam.Type],
                int.Parse(candidate[(int)StdParam.Bar]),
                int.Parse(candidate[(int)StdParam.Tick]),
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.LastTime], int.Parse(candidate[(int)StdParam.WaitTimeOrParam]),
                int.Parse(candidate[(int)StdParam.EndKey]),
                "M1"); //candidate[(int)StdParam.EndKey] is special effect
            }
            else
                return new Hold(candidate[(int)StdParam.Type],
                            int.Parse(candidate[(int)StdParam.Bar]),
                            int.Parse(candidate[(int)StdParam.Tick]),
                            candidate[(int)StdParam.KeyOrParam],
                            int.Parse(candidate[(int)StdParam.WaitTimeOrParam]));
        }

        public Slide SlideOfToken(string token, int bar, int tick, Note slideStart, double bpm)
        {
            Note result;
            string[] candidate = token.Split('\t');
            result = new Slide(candidate[(int)StdParam.Type],
                                   bar,
                                   tick,
                                   slideStart.Key,
                                   int.Parse(candidate[(int)StdParam.WaitTimeOrParam]),
                                   int.Parse(candidate[(int)StdParam.LastTime]),
                                   candidate[(int)StdParam.EndKey]);
            if (!slideStart.Key.Equals(candidate[(int)StdParam.KeyOrParam]))
            {
                throw new Exception("THE SLIDE START DOES NOT MATCH WITH THE DEFINITION OF THIS NOTE!");
            }
            result.BPM = bpm;
            return (Slide)result;
        }

        public Slide SlideOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = int.Parse(candidate[(int)StdParam.Bar]);
            int tick = int.Parse(candidate[(int)StdParam.Tick]);
            if (!PreviousSlideStart.Key.Equals(candidate[(int)StdParam.KeyOrParam]))
            {
                Console.WriteLine("Expected key: " + candidate[(int)StdParam.KeyOrParam]);
                Console.WriteLine("Actual key: " + PreviousSlideStart.Key);
                Console.WriteLine("Previous Slide Start: " + PreviousSlideStart.Compose((int)StdParam.Bar));
                throw new Exception("THE SLIDE START DOES NOT MATCH WITH THE DEFINITION OF THIS NOTE!");
            }
            return new Slide(candidate[(int)StdParam.Type],
                        bar,
                        tick,
                        PreviousSlideStart.Key,
                        int.Parse(candidate[(int)StdParam.WaitTimeOrParam]),
                        int.Parse(candidate[(int)StdParam.LastTime]),
                        candidate[(int)StdParam.EndKey]);
        }


        public Tap TapOfToken(string token, int bar, int tick, double bpm)
        {
            Note result = new Rest();
            string[] candidate = token.Split('\t');
            if (candidate[(int)StdParam.Type].Equals("TTP") && (candidate.Count()) >= 7)
            {
                result = new Tap(candidate[(int)StdParam.Type],
                bar,
                tick,
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.WaitTimeOrParam],
                int.Parse(candidate[(int)StdParam.LastTime]),
                candidate[(int)StdParam.EndKey]);
            }
            else if (candidate[(int)StdParam.Type].Equals("TTP") && (candidate.Count()) < 7)
            {
                //Console.ReadLine();
                result = new Tap(candidate[(int)StdParam.Type],
                int.Parse(candidate[(int)StdParam.Bar]),
                int.Parse(candidate[(int)StdParam.Tick]),
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.WaitTimeOrParam],
                int.Parse(candidate[(int)StdParam.LastTime]),
                "M1");
            }
            else
                result = new Tap(candidate[(int)StdParam.Type],
                    int.Parse(candidate[(int)StdParam.Bar]),
                    int.Parse(candidate[(int)StdParam.Tick]),
                    candidate[(int)StdParam.KeyOrParam]);
            result.BPM = bpm;
            return (Tap)result;
        }

        public Tap TapOfToken(string token)
        {
            string[] candidate = token.Split('\t');
            int bar = int.Parse(candidate[(int)StdParam.Bar]);
            int tick = int.Parse(candidate[(int)StdParam.Tick]);
            if (candidate[(int)StdParam.Type].Equals("TTP") && (candidate.Count()) >= 7)
            {
                return new Tap(candidate[(int)StdParam.Type],
                bar,
                tick,
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.WaitTimeOrParam],
                int.Parse(candidate[(int)StdParam.LastTime]),
                candidate[(int)StdParam.EndKey]);
            }
            else if (candidate[(int)StdParam.Type].Equals("TTP") && (candidate.Count()) < 7)
            {
                //Console.ReadLine();
                return new Tap(candidate[(int)StdParam.Type],
                int.Parse(candidate[(int)StdParam.Bar]),
                int.Parse(candidate[(int)StdParam.Tick]),
                candidate[(int)StdParam.KeyOrParam] + candidate[(int)StdParam.WaitTimeOrParam],
                int.Parse(candidate[(int)StdParam.LastTime]),
                "M1");
            }
            else
                return new Tap(candidate[(int)StdParam.Type],
                    int.Parse(candidate[(int)StdParam.Bar]),
                    int.Parse(candidate[(int)StdParam.Tick]),
                    candidate[(int)StdParam.KeyOrParam]);
        }
    }

}

