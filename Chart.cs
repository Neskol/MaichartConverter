namespace MaidataConverter
{
    public abstract class Chart : IChart
    {
        //Stores all notes
        private List<Note> notes;

        //Stores definitions of BPM Changes
        private BPMChanges bpmChanges;

        //Stores definitions of Measure Changes
        private MeasureChanges measureChanges;

        //Counts number of Tap
        private int tapNumber;

        //Counts number of Break
        private int breakNumber;

        //Counts number of Hold
        private int holdNumber;

        //Counts number of Slide
        private int slideNumber;

        //Counts number of Touch
        private int touchNumber;

        //Counts number of Touch Hold
        private int thoNumber;

        //Defines 
        private int[] unitScore = { 500, 1000, 1500, 2500 };
        private int achievement = 0;
        private int totalDelay = 0;
        private List<List<Note>> chart;
        private Dictionary<string, string> information;
        private readonly string[] TapTypes = { "TAP", "STR", "TTP", "XTP", "XST" };
        private readonly string[] HoldTypes = { "HLD", "THO", "XHO" };
        private readonly string[] SlideTypes = { "SI_", "SV_", "SF_", "SCL", "SCR", "SUL", "SUR", "SLL", "SLR", "SXL", "SXR", "SSL", "SSR" };

        ///Theoritical Rating = (Difference in 100-down and Max score)/100-down
        /// <summary>
        /// Access to Notes
        /// </summary>
        public List<Note> Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes=value;
            }
        }

        public List<List<Note>> StoredChart
        {
            get 
            { 
                return this.chart; 
            }
            set 
            {
                this.chart=value;
            }
        }

        /// <summary>
        /// Access to BPM Changes
        /// </summary>
        public BPMChanges BPMChanges
        {
            get
            {
                return this.bpmChanges;
            }
            set
            {
                this.bpmChanges = value;
            }
        }

        /// <summary>
        /// Access to Measure Changes
        /// </summary>
        public MeasureChanges MeasureChanges
        {
            get
            {
                return this.measureChanges;
            }
            set
            {
                this.measureChanges = value;
            }
        }

        /// <summary>
        /// Access to Tap Number
        /// </summary>
        public int TapNumber
        {
            get
            {
                return this.tapNumber;
            }
            set
            {
                this.tapNumber = value;
            }
        }

        /// <summary>
        /// Access to Break Number
        /// </summary>
        public int BreakNumber
        {
            get
            {
                return this.breakNumber;
            }
            set
            {
                this.breakNumber = value;
            }
        }

        /// <summary>
        /// Access to Hold Number
        /// </summary>
        public int HoldNumber
        {
            get
            {
                return this.holdNumber;
            }
            set
            {
                this.holdNumber = value;
            }
        }

        /// <summary>
        /// Access to Slide Number
        /// </summary>
        public int SlideNumber
        {
            get
            {
                return this.slideNumber;
            }
            set
            {
                this.slideNumber = value;
            }
        }

        /// <summary>
        /// Access to Touch Number
        /// </summary>
        public int TouchNumber
        {
            get
            {
                return this.touchNumber;
            }
            set
            {
                this.touchNumber = value;
            }
        }

        /// <summary>
        /// Access to Touch Hold Number
        /// </summary>
        public int ThoNumber
        {
            get
            {
                return this.thoNumber;
            }
            set
            {
                this.thoNumber = value;
            }
        }

        /// <summary>
        /// Access to Unit Score
        /// </summary>
        public int[] UnitScore
        {
            get
            {
                return this.unitScore;
            }
        }

        /// <summary>
        /// Access to theoritical Achievement
        /// </summary>
        public int Achievement
        {
            get
            {
                return this.achievement;
            }
            set
            {
                this.achievement = value;
            }
        }

        public int TotalDelay
        {
            get
            {
                return this.totalDelay;
            }
            set
            {
                this.totalDelay = value;
            }
        }

        public Dictionary<string,string> Information
        {
            get
            {
                return this.information;
            }
            set
            {
                this.information = value;
            }
        }

        public Chart()
        {
            this.notes = new List<Note>();
            this.bpmChanges = new BPMChanges();
            this.measureChanges = new MeasureChanges();
            this.chart = new List<List<Note>>();
            this.information = new Dictionary<string, string>();
        }

        /// <summary>
        /// Construct Good Brother with given notes, bpm change definitions and measure change definitions.
        /// </summary>
        /// <param name="notes">Notes in Good Brother</param>
        /// <param name="bpmChanges">BPM Changes: Initial BPM is NEEDED!</param>
        /// <param name="measureChanges">Measure Changes: Initial Measure is NEEDED!</param>
        public Chart(List<Note> notes, BPMChanges bpmChanges, MeasureChanges measureChanges)
        {
            this.notes = notes;
            this.bpmChanges = bpmChanges;
            this.measureChanges = measureChanges;
            this.chart = new List<List<Note>>();
            this.information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother from location specified
        /// </summary>
        /// <param name="location">MA2 location</param>
        public Chart(string location)
        {
            string[] tokens = new Tokenizer().Tokens(location);
            Ma2 takenIn = new Ma2parser().GoodBrotherOfToken(tokens);
            this.notes = takenIn.Notes;
            this.bpmChanges = takenIn.BPMChanges;
            this.measureChanges = takenIn.MeasureChanges;
            this.chart = new List<List<Note>>();
            this.information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother with tokens given
        /// </summary>
        /// <param name="tokens">Tokens given</param>
        public Chart(string[] tokens)
        {
            Ma2 takenIn = new Ma2parser().GoodBrotherOfToken(tokens);
            this.notes = takenIn.Notes;
            this.bpmChanges = takenIn.BPMChanges;
            this.measureChanges = takenIn.MeasureChanges;
            this.chart = new List<List<Note>>();
            this.information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Construct GoodBrother with existing values
        /// </summary>
        /// <param name="takenIn">Existing good brother</param>
        public Chart(Ma2 takenIn)
        {
            this.notes = takenIn.Notes;
            this.bpmChanges = takenIn.BPMChanges;
            this.measureChanges = takenIn.MeasureChanges;
            this.chart = new List<List<Note>>();
            this.information = new Dictionary<string, string>();
            this.Update();
        }

        /// <summary>
        /// Check if every item is valid for exporting
        /// </summary>
        /// <returns>True if every element is valid, false elsewise</returns>
        public bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update properties in Good Brother for exporting
        /// </summary>
        public void Update()
        {
            int maxBar = notes[notes.Count - 1].Bar;
            for (int i = 0; i <= maxBar; i++)
            {
                List<Note> bar = new List<Note>();
                BPMChange noteChange = new BPMChange();
                double currentBPM = this.BPMChanges.ChangeNotes[0].BPM;
                Note lastNote = new Rest();
                foreach (BPMChange x in this.BPMChanges.ChangeNotes)
                {
                    if (x.Bar == i)
                    {
                        bar.Add(x);
                    }
                }
                foreach (Note x in this.Notes)
                {
                    if (x.Bar == i)
                    {
                        int delay = x.Bar * 384 + x.StartTime + x.WaitTime + x.LastTime;
                        switch (x.NoteSpecificType())
                        {
                            case "BPM":
                                currentBPM = x.BPM;
                                break;
                            case "MEASURE":
                                break;
                            case "REST":
                                break;
                            case "TAP":
                                this.tapNumber++;
                                if (x.NoteType.Equals("TTP"))
                                {
                                    this.touchNumber++;
                                }
                                else if (x.NoteType.Equals("BRK") || x.NoteType.Equals("BST"))
                                {
                                    this.breakNumber++;
                                }
                                break;
                            case "HOLD":
                                this.holdNumber++;
                                this.slideNumber++;
                                if (delay > this.TotalDelay)
                                {
                                    this.totalDelay = delay;
                                    //Console.WriteLine("New delay: " + delay);
                                    //Console.WriteLine(x.Compose(1));
                                }
                                if (x.NoteType.Equals("THO"))
                                {
                                    this.thoNumber++;
                                }
                                break;
                            case "SLIDE_START":
                                this.tapNumber++;
                                break;
                            case "SLIDE":
                                this.slideNumber++;
                                if (delay > this.TotalDelay)
                                {
                                    this.totalDelay = delay;
                                    //Console.WriteLine("New delay: "+delay);
                                    //Console.WriteLine(x.Compose(1));
                                }
                                break;
                            default:
                                break;
                        }
                        x.BPM = currentBPM;
                        x.Prev = lastNote;
                        lastNote.Next = x;
                        bar.Add(x);
                        if (!x.NoteSpecificType().Equals("SLIDE"))
                        {
                            lastNote = x;
                        }
                    }
                }

                List<Note> afterBar = new List<Note>();
                afterBar.Add(new MeasureChange(i, 0, CalculateQuaver(CalculateLeastMeasure(bar))));
                //Console.WriteLine();
                //Console.WriteLine("In bar "+i+", LeastMeasure is "+ CalculateLeastMeasure(bar)+", so quaver will be "+ CalculateQuaver(CalculateLeastMeasure(bar)));
                afterBar.AddRange(bar);
                this.chart.Add(FinishBar(afterBar, this.BPMChanges.ChangeNotes, i, CalculateQuaver(CalculateLeastMeasure(bar))));
            }
            //Console.WriteLine("TOTAL DELAY: "+this.TotalDelay);
            //Console.WriteLine("TOTAL COUNT: "+ this.chart.Count * 384);
            if (this.totalDelay<this.chart.Count*384)
            {
                this.totalDelay = 0;
            }
            else
            {
                this.totalDelay -= this.chart.Count * 384;
            }
        }

        /// <summary>
        /// Compose chart in appropriate result.
        /// </summary>
        /// <returns>String of chart compiled</returns>
        public abstract string Compose();

        /// <summary>
        /// Override and compose with given arrays
        /// </summary>
        /// <param name="bpm">Override BPM array</param>
        /// <param name="measure">Override Measure array</param>
        /// <returns>Good Brother with override array</returns>
        public abstract string Compose(BPMChanges bpm, MeasureChanges measure);

        /// <summary>
        /// Return the least none 0 measure of bar.
        /// </summary>
        /// <param name="bar">bar to take in</param>
        /// <returns>List none 0 measure</returns>
        public static int CalculateLeastMeasure(List<Note> bar)
        {
            List<int> startTimeList = new List<int>();
            startTimeList.Add(0);
            foreach (Note x in bar)
            {
                if (!startTimeList.Contains(x.StartTime))
                {
                    startTimeList.Add(x.StartTime);
                }
                if (x.NoteType.Equals("BPM"))
                {
                    //Console.WriteLine(x.Compose(0));
                }
            }
            if (startTimeList[startTimeList.Count - 1] != 384)
            {
                startTimeList.Add(384);
            }
            List<int> intervalCandidates = new List<int>();
            int minimalInterval = GCD(startTimeList[0],startTimeList[1]);
            for (int i = 1; i < startTimeList.Count; i++)
            {
                minimalInterval = GCD(minimalInterval, startTimeList[i]);
            }
            //if (intervalCandidates.Min() == 0)
            //{
            //    throw new Exception("Error: Least interval was 0");
            //}
            //int minimalInterval = intervalCandidates.Min();
            //if (minimalInterval == 0)
            //{
            //    throw new Exception("Error: Note number does not match in bar " + bar[0].Bar);
            //}
            //bool primeInterval = false;
            //bool notAllDivisible = true;
            //foreach (int num in intervalCandidates)
            //{
            //    notAllDivisible = notAllDivisible || num % minimalInterval != 0;
            //    if (IsPrime(num))
            //    {
            //        minimalInterval = 1;
            //        primeInterval = true;
            //        notAllDivisible = false;
            //    }
            //    else if (!primeInterval)
            //    {
            //        if (minimalInterval != 0 && (num % minimalInterval) != 0)
            //        {
            //            if (GCD(num, minimalInterval) != 1)
            //            {
            //                minimalInterval /=minimalInterval% GCD(minimalInterval, num);
            //            }
            //            else
            //            {
            //                minimalInterval = 1;
            //                primeInterval = true;
            //                notAllDivisible = false;
            //            }
            //        }
            //    }
            //}
            //Console.WriteLine("Minimal Interval: "+minimalInterval);
            return minimalInterval;
            //return 1;
        }

        /// <summary>
        /// Return note number except Rest, BPM and Measure.
        /// </summary>
        /// <param name="Bar">bar of note to take in</param>
        /// <returns>Number</returns>
        public static int RealNoteNumber(List<Note> Bar)
        {
            int result = 0;
            foreach (Note x in Bar)
            {
                if (x.IsNote())
                {
                    result++;
                }
            }
            return result;
        }


        public static bool ContainNotes(List<Note> Bar)
        {
            bool result = false;
            foreach (Note x in Bar)
            {
                result = result || x.IsNote();
            }
            return result;
        }

        /// <summary>
        /// Generate appropriate length for hold and slide.
        /// </summary>
        /// <param name="length">Last Time</param>
        /// <returns>[Definition:Length]=[Quaver:Beat]</returns>
        public static int CalculateQuaver(int length)
        {
            int result = 0;
            const int definition = 384;
            int divisor = GCD(definition, length);
            int quaver = definition / divisor, beat = length / divisor;
            result = quaver;
            return result;
        }

        /// <summary>
        /// Finish Bar writing byu adding specific rest note in between.
        /// </summary>
        /// <param name="bar">Bar to finish with</param>
        /// <param name="bpmChanges">BPMChange Notes</param>
        /// <param name="barNumber">Bar number of Bar</param>
        /// <param name="minimalQuaver">Minimal interval calculated from bar</param>
        /// <returns>Finished bar</returns>
        public static List<Note> FinishBar(List<Note> bar, List<BPMChange> bpmChanges, int barNumber, int minimalQuaver)
        {
            List<Note> result = new List<Note>();
            //Console.WriteLine("The taken in minimal interval is "+minimalQuaver);
            //foreach (BPMChange x in bpmChanges)
            //{
            //    if (x.Bar == barNumber && x.NoteGenre().Equals("BPM"))
            //    {
            //        result.Add(x);
            //        Console.WriteLine("A BPMChange was found and locate in bar" + x.Bar + " in tick " + x.StartTime);
            //    }
            //} 
            bool writeRest = true;
            result.Add(bar[0]);
            for (int i = 0; i < 384; i += 384 / minimalQuaver)
            {
                List<Note> eachSet = new List<Note>();
                List<Note> touchEachSet = new List<Note>();
                writeRest = true;
                foreach (Note x in bar)
                {
                    //Console.Write("c1: " + (x.StartTime == i));
                    //Console.Write("; c2: " + (x.IsNote()));
                    //Console.Write("; c3: " + (x.NoteSpecificType().Equals("MEASURE")));
                    //Console.Write("; FINAL: " + ((x.StartTime == i && x.IsNote()) || x.NoteSpecificType().Equals("MEASURE")));

                    if ((x.StartTime == i) && x.IsNote() && !(x.NoteType.Equals("TTP")|| x.NoteType.Equals("THO")))
                    {
                        if (x.NoteSpecificType().Equals("BPM"))
                        {
                            eachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                        }
                        else
                        {
                            eachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                            writeRest = false;
                        }                      
                    }
                    else if ((x.StartTime == i) && x.IsNote() && (x.NoteType.Equals("TTP") || x.NoteType.Equals("THO")))
                    {
                        if (x.NoteSpecificType().Equals("BPM"))
                        {
                            touchEachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                        }
                        else
                        {
                            touchEachSet.Add(x);
                            //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                            writeRest = false;
                        }
                    }

                    //if ((x.StartTime == i) && x.IsNote())
                    //{
                    //    eachSet.Add(x);
                    //    //Console.WriteLine("A note was found at tick " + i + " of bar " + barNumber + ", it is "+x.NoteType);
                    //    writeRest = false;
                    //}
                }
                bool addedTouch = false;
                foreach (BPMChange x in bpmChanges)
                {
                    if (eachSet.Contains(x)&&!addedTouch)
                    {
                        eachSet.Remove(x);
                        List<Note> adjusted = new List<Note>();
                        adjusted.Add(x);
                        adjusted.AddRange(touchEachSet);
                        adjusted.AddRange(eachSet);
                        eachSet = adjusted;
                        addedTouch = true;
                    }
                    else if (!addedTouch)
                    {
                        List<Note> adjusted = new List<Note>();
                        adjusted.AddRange(touchEachSet);
                        adjusted.AddRange(eachSet);
                        eachSet = adjusted;
                        addedTouch= true;
                    }
                }
                //for (int index = 0;index<eachSet.Count;index++)
                //{
                //    if (eachSet[index].NoteSpecificType().Equals("BPM"))
                //    {
                //        List<Note> adjusted = new List<Note>();
                //        adjusted.Add(eachSet[index]);
                //        eachSet.RemoveAt(index);
                //        adjusted.AddRange(eachSet);
                //        eachSet = adjusted;
                //    }
                //}
                if (writeRest)
                {
                    //Console.WriteLine("There is no note at tick " + i + " of bar " + barNumber + ", Adding one");
                    eachSet.Add(new Rest("RST", barNumber, i));
                }
                result.AddRange(eachSet);
            }
            if (RealNoteNumber(result) != RealNoteNumber(bar))
            {
                string error = "";
                error += ("Bar notes not match in bar: " + barNumber)+"\n";
                error += ("Expected: " + RealNoteNumber(bar)) + "\n";
                foreach (Note x in bar)
                {
                    error += (x.Compose(1)) + "\n";
                }
                error += ("\nActrual: " + RealNoteNumber(result)) + "\n";
                foreach (Note y in result)
                {
                    error += (y.Compose(1)) + "\n";
                }
                Console.WriteLine(error);
                throw new Exception("NOTE NUMBER IS NOT MATCHING");
            }
            //result.Sort();
            //if (RealNoteNumber(result)==0)
            //{
            //    Console.WriteLine("There is no note at tick " + 0 + " of bar " + barNumber + ", Adding one");
            //    result.Add(new Rest("RST", barNumber,0));
            //}
            // if (result[1].NoteSpecificType().Equals("BPM"))
            // {
            //     Note temp = result[0];
            //     result[0] = result[1];
            //     result[1] = temp;
            // }
            bool hasFirstBPMChange=false;
            List<Note> changedResult = new List<Note>();
            Note potentialFirstChange = new Rest();
            {
                for(int i = 0;!hasFirstBPMChange&&i<result.Count();i++)
                {
                    if (result[i].NoteGenre().Equals("BPM")&&result[i].StartTime==0)
                    {                    
                        changedResult.Add(result[i]);
                        potentialFirstChange = result[i];
                        hasFirstBPMChange=true;
                    }
                }
                if (hasFirstBPMChange)
                {
                    result.Remove(potentialFirstChange);
                    changedResult.AddRange(result);
                    result=changedResult;
                }
            }

            return result;
        }

        /// <summary>
        /// Return GCD of A and B.
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <returns>GCD of A and B</returns>
        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        /// <summary>
        /// Return if this is a prime (1 counts)
        /// </summary>
        /// <param name="number">Number to inspect</param>
        /// <returns>True if is prime, false elsewise</returns>
        public static bool IsPrime(int number)
        {
            if (number < 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Take in and replace the current information.
        /// </summary>
        /// <param name="information">Dicitionary containing information needed</param>
        public void TakeInformation(Dictionary<string, string> information)
        {
            foreach (KeyValuePair<string, string> x in information)
            {
                this.information.Add(x.Key, x.Value);
            }
        }
    }
}