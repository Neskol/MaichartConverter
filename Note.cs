using System.Numerics;
using System.Resources;

namespace MaichartConverter
{
    /// <summary>
    /// Basic note
    /// </summary>
    public abstract class Note : IEquatable<Note>, INote, IComparable
    {
        /// <summary>
        /// The note type
        /// </summary>
        private string noteType;

        /// <summary>
        /// The key
        /// </summary>
        private string key;

        /// <summary>
        /// The end key
        /// </summary>
        private string endKey;

        /// <summary>
        /// The bar
        /// </summary>
        private int bar;

        /// <summary>
        /// The start time
        /// </summary>
        private int tick;

        /// <summary>
        /// The absolute tick calculated by this.bar*384+this.tick
        /// </summary>
        private int tickStamp;

        /// <summary>
        /// The start time stamp
        /// </summary>
        private double tickTimeStamp;

        /// <summary>
        /// The wait length
        /// </summary>
        private int waitLength;

        /// <summary>
        /// The stamp of wait time ends in ticks
        /// </summary>
        private int waitTickStamp;

        /// <summary>
        /// The stamp when the wait time ends in seconds
        /// </summary>
        private double waitTimeStamp;

        /// <summary>
        /// The calculated wait time in seconds
        /// </summary>
        private double calculatedWaitTime;

        /// <summary>
        /// The last length
        /// </summary>
        private int lastLength;

        /// <summary>
        /// The stamp when the last time ends in ticks
        /// </summary>
        private int lastTickStamp;

        /// <summary>
        /// The stamp when the last time ends in seconds
        /// </summary>
        private double lastTimeStamp;

        /// <summary>
        /// The calculated last time
        /// </summary>
        private double calculatedLastTime;

        /// <summary>
        /// Stores if the BPM of wait or last tick is in different BPM
        /// </summary>
        private bool tickBPMDisagree;

        /// <summary>
        /// The delayed
        /// </summary>
        private bool delayed;

        /// <summary>
        /// The BPM
        /// </summary>
        private double bpm;

        /// <summary>
        /// The previous
        /// </summary>
        private Note? prev;

        /// <summary>
        /// The next
        /// </summary>
        private Note? next;

        /// <summary>
        /// Stores the start note of slide
        /// </summary>
        private Note? slideStart;

        /// <summary>
        /// Stores the connecting slide of slide start
        /// </summary>
        private Note? consecutiveSlide;

        /// <summary>
        /// Stores all BPM change prior to this
        /// </summary>
        private List<BPMChange> bpmChangeNotes;

        /// <summary>
        /// Construct an empty note
        /// </summary>
        public Note()
        {
            noteType = "";
            key = "";
            endKey = "";
            bar = 0;
            tick = 0;
            tickStamp = 0;
            tickTimeStamp = 0.0;
            lastLength = 0;
            lastTickStamp = 0;
            lastTimeStamp = 0.0;
            waitLength = 0;
            waitTickStamp = 0;
            waitTimeStamp = 0.0;
            calculatedLastTime = 0.0;
            calculatedWaitTime = 0.0;
            tickBPMDisagree = false;
            bpm = 0;
            bpmChangeNotes = new List<BPMChange>();
        }

        /// <summary>
        /// Construct a note from other note
        /// </summary>
        /// <param name="inTake">The intake note</param>
        public Note(Note inTake)
        {
            this.noteType = inTake.NoteType;
            this.key = inTake.Key;
            this.endKey = inTake.EndKey;
            this.bar = inTake.Bar;
            this.tick = inTake.Tick;
            this.tickStamp = inTake.TickStamp;
            this.tickTimeStamp = inTake.TickTimeStamp;
            this.lastLength = inTake.LastLength;
            this.lastTickStamp = inTake.LastTickStamp;
            this.lastTimeStamp = inTake.LastTimeStamp;
            this.waitLength = inTake.WaitLength;
            this.waitTickStamp = inTake.WaitTickStamp;
            this.waitTimeStamp = inTake.WaitTimeStamp;
            this.calculatedLastTime = inTake.CalculatedLastTime;
            this.calculatedLastTime = inTake.CalculatedLastTime;
            this.tickBPMDisagree = inTake.TickBPMDisagree;
            this.bpm = inTake.BPM;
            this.bpmChangeNotes = inTake.bpmChangeNotes;
        }

        /// <summary>
        /// Access NoteType
        /// </summary>
        public string NoteType
        {
            get
            {
                return this.noteType;
            }
            set
            {
                this.noteType = value;
            }
        }

        /// <summary>
        /// Access Key
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// Access Bar
        /// </summary>
        public int Bar
        {
            get
            {
                return this.bar;
            }
            set
            {
                this.bar = value;
            }
        }

        /// <summary>
        /// Access Tick
        /// </summary>
        public int Tick
        {
            get
            {
                return this.tick;
            }
            set
            {
                this.tick = value;
            }
        }

        /// <summary>
        /// Access Tick Stamp = this.Bar*384 + this.Tick
        /// </summary>
        public int TickStamp
        {
            get { return this.tickStamp; }
            set { this.tickStamp = value; }
        }

        /// <summary>
        /// Access Tick Stamp = this.Bar*384 + this.Tick
        /// </summary>
        public double TickTimeStamp
        {
            get { return this.tickTimeStamp; }
            set { this.tickTimeStamp = value; }
        }

        /// <summary>
        /// Access wait time
        /// </summary>
        public int WaitLength
        {
            get
            {
                return this.waitLength;
            }
            set
            {
                this.waitLength = value;
            }
        }

        /// <summary>
        /// Access the time stamp where wait time ends in ticks
        /// </summary>
        /// <value>The incoming time</value>
        public int WaitTickStamp
        {
            get { return this.waitTickStamp; }
            set { this.waitTickStamp = value; }
        }

        /// <summary>
        /// Access the time stamp where wait time ends in seconds
        /// </summary>
        /// <value>The incoming time</value>
        public double WaitTimeStamp
        {
            get { return this.waitTimeStamp; }
            set { this.waitTimeStamp = value; }
        }

        /// <summary>
        /// Gets or sets the calculated wait time.
        /// </summary>
        /// <value>
        /// The calculated wait time in seconds.
        /// </value>
        public double CalculatedWaitTime
        {
            get { return this.calculatedWaitTime; }
            set { this.calculatedWaitTime = value; }
        }

        /// <summary>
        /// Access EndTime
        /// </summary>
        public int LastLength
        {
            get
            {
                return this.lastLength;
            }
            set
            {
                this.lastLength = value;
            }
        }

        /// <summary>
        /// Access Last time in ticks
        /// </summary>
        public int LastTickStamp
        {
            get
            {
                return this.lastTickStamp;
            }
            set
            {
                this.lastTickStamp = value;
            }
        }

        /// <summary>
        /// Access last time in seconds
        /// </summary>
        public double LastTimeStamp
        {
            get
            {
                return this.lastTimeStamp;
            }
            set
            {
                this.lastTimeStamp = value;
            }
        }

        /// <summary>
        /// Gets or sets the calculated last time in seconds.
        /// </summary>
        /// <value>
        /// The calculated last time in seconds.
        /// </value>
        public double CalculatedLastTime
        {
            get => this.calculatedLastTime;
            set { this.calculatedLastTime = value; }
        }

        /// <summary>
        /// Stores if the wait or last are in different BPM
        /// </summary>
        /// <value>True if in different BPM, false elsewise</value>
        public bool TickBPMDisagree
        {
            get => this.tickBPMDisagree;
            set { this.tickBPMDisagree = value; }
        }

        /// <summary>
        /// Access EndKey
        /// </summary>
        public string EndKey
        {
            get
            {
                return this.endKey;
            }
            set
            {
                this.endKey = value;
            }
        }

        /// <summary>
        /// Access Delayed
        /// </summary>
        public bool Delayed
        {
            get { return this.delayed; }
            set { this.delayed = value; }
        }

        /// <summary>
        /// Access BPM
        /// </summary>
        public double BPM
        {
            get { return this.bpm; }
            set { this.bpm = value; }
        }

        /// <summary>
        /// Access this.prev;
        /// </summary>
        public Note? Prev
        {
            get { return this.prev; }
            set { this.prev = value; }
        }

        /// <summary>
        /// Access this.next
        /// </summary>
        public Note? Next
        {
            get { return this.next; }
            set { this.next = value; }
        }

        /// <summary>
        /// Return the slide start of a note (reserved for slides only)
        /// </summary>
        public Note? SlideStart
        {
            get { return this.slideStart; }
            set { this.slideStart = value; }
        }

        /// <summary>
        /// Return the consecutive of a note (reserved for slides only)
        /// </summary>
        public Note? ConsecutiveSlide
        {
            get { return this.consecutiveSlide; }
            set { this.consecutiveSlide = value; }
        }

        public List<BPMChange> BPMChangeNotes
        {
            get
            {
                return this.bpmChangeNotes;
            }
            set
            {
                this.bpmChangeNotes = value;
            }
        }

        /// <summary>
        /// Return this.SpecificType
        /// </summary>
        /// <returns>string of specific genre (specific type of Tap, Slide, etc.)</returns>
        public abstract string NoteSpecificType { get; }

        /// <summary>
        /// Return this.noteGenre
        /// </summary>
        /// <returns>string of note genre (general category of TAP, SLIDE and HOLD)</returns>
        public abstract string NoteGenre { get; }

        /// <summary>
        /// Return if this is a true note
        /// </summary>
        /// <returns>True if is TAP,HOLD or SLIDE, false elsewise</returns>
        public abstract bool IsNote { get; }

        public abstract bool CheckValidity();

        public abstract string Compose(int format);

        public int CompareTo(Object? obj)
        {
            int result = 0;

            Note another = obj as Note ?? throw new NullReferenceException("Note is not defined");

            //else if (this.NoteSpecificType().Equals("SLIDE")&&(this.NoteSpecificType().Equals("TAP")|| this.NoteSpecificType().Equals("HOLD")) && this.tick == another.Tick && this.bar == another.Bar)
            //{
            //    result = -1;
            //}
            //else if (this.NoteSpecificType().Equals("SLIDE_START") && (another.NoteSpecificType().Equals("TAP") || another.NoteSpecificType().Equals("HOLD")) && this.tick == another.Tick && this.bar == another.Bar)
            //{
            //    Console.WriteLine("STAR AND TAP");
            //    result = 1;
            //    Console.WriteLine(this.NoteSpecificType() + ".compareTo(" + another.NoteSpecificType() + ") is" + result);
            //    //Console.ReadKey();
            //}
            //if (this.Bar==another.Bar&&this.Tick==another.Tick)
            //{
            //    if (this.NoteGenre().Equals("BPM"))
            //    {
            //        result = -1;
            //    }
            //    else if (this.NoteGenre().Equals("MEASURE"))
            //    {
            //        result = 1;
            //    }
            //    else if ((this.NoteSpecificType().Equals("TAP")|| this.NoteSpecificType().Equals("HOLD"))&&another.NoteSpecificType().Equals("SLIDE_START"))
            //    {
            //        result= -1;
            //    }
            //}
            //else
            //{
            //    if (this.bar != another.Bar)
            //    {
            //        result = this.bar.CompareTo(another.Bar);
            //        //Console.WriteLine("this.compareTo(another) is" + result);
            //        //Console.ReadKey();
            //    }
            //    else result = this.tick.CompareTo(another.Tick);
            //}
            if (this.Bar != another.Bar)
            {
                result = this.Bar.CompareTo(another.Bar);
            }
            else if (this.Bar == another.Bar && (this.Tick != another.Tick))
            {
                result = this.Tick.CompareTo(another.Tick);
            }
            else
            {
                if (this.NoteSpecificType.Equals("BPM"))
                {
                    result = -1;
                }
                //else if (this.NoteSpecificType().Equals("SLIDE")&&another.NoteSpecificType().Equals("SLIDE_START")&&this.Key.Equals(another.Key))
                //{
                //    result = 1;
                //}
                else result = 0;
            }
            return result;
        }

        public bool Equals(Note? other)
        {
            bool result = false;
            if (other != null &&
            this.NoteType.Equals(other.NoteType) &&
            this.Key.Equals(other.Key) &&
            this.EndKey.Equals(other.EndKey) &&
            this.Bar == other.Bar &&
            this.Tick == other.Tick &&
            this.LastLength == other.LastLength &&
            this.BPM == other.BPM)
            {
                result = true;
            }
            return result;
        }

        public bool Update()
        {
            // Console.WriteLine("This note has bpm note number of " + this.BPMChangeNotes.Count());
            bool result = false;
            this.tickStamp = this.bar * 384 + this.tick;
            // string noteInformation = "This note is "+this.NoteType+", in tick "+ this.tickStamp+", ";
            //this.tickTimeStamp = this.GetTimeStamp(this.tickStamp);
            this.waitTickStamp = this.tickStamp + this.waitLength;
            //this.waitTimeStamp = this.GetTimeStamp(this.waitTickStamp);
            this.lastTickStamp = this.waitTickStamp + this.lastLength;
            //this.lastTimeStamp = this.GetTimeStamp(this.lastTickStamp);
            if (!(this.NoteType.Equals("SLIDE") || this.NoteType.Equals("HOLD")))
            {
                result = true;
            }
            else if (this.calculatedLastTime > 0 && this.calculatedWaitTime > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Replace this.BPMChangeNotes from change table given
        /// </summary>
        /// <param name="changeTable">Change table contains bpm notes</param>
        public void ReplaceBPMChanges(BPMChanges changeTable)
        {
            this.bpmChangeNotes = new List<BPMChange>();
            this.bpmChangeNotes.AddRange(changeTable.ChangeNotes);
        }

        /// <summary>
        /// Replace this.BPMChangeNotes from change table given
        /// </summary>
        /// <param name="changeTable">Change table contains bpm notes</param>
        public void ReplaceBPMChanges(List<BPMChange> changeTable)
        {
            this.bpmChangeNotes = new List<BPMChange>();
            this.bpmChangeNotes.AddRange(changeTable);
        }

        /// <summary>
        /// Generate appropriate length for hold and slide.
        /// </summary>
        /// <param name="length">Last Time</param>
        /// <returns>[Definition:Length]=[Quaver:Beat]</returns>
        public string GenerateAppropriateLength(int length)
        {
            string result = "";
            const int definition = 384;
            int divisor = GCD(definition, length);
            int quaver = definition / divisor, beat = length / divisor;
            result = "[" + quaver.ToString() + ":" + beat.ToString() + "]";
            return result;
        }

        /// <summary>
        /// Return GCD of A and B.
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <returns>GCD of A and B</returns>
        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        /// <summary>
        /// Generate appropriate length for hold and slide.
        /// </summary>
        /// <param name="length">Last Time</param>
        /// <param name="bpm">BPM</param>
        /// <returns>[Definition:Length]=[Quaver:Beat]</returns>
        public string GenerateAppropriateLength(int length, double bpm)
        {
            string result = "";
            double sustain = this.WaitTimeStamp - this.TickTimeStamp;
            double duration = this.LastTimeStamp - this.WaitTimeStamp;
            result = "[" + sustain + "##" + duration + "]";
            return result;
        }

        /// <summary>
        /// Get BPM Time tick unit of bpm
        /// </summary>
        /// <param name="bpm">BPM to calculate</param>
        /// <returns>BPM Tick Unit of bpm</returns>
        public static double GetBPMTimeUnit(double bpm)
        {
            double result = 60 / bpm * 4 / 384;
            return result;
        }

        public double GetTimeStamp(int overallTick)
        {
            double result = 0.0;
            if (overallTick != 0)
            {
                int maximumBPMIndex = 0;
                for (int i = 0; i < this.bpmChangeNotes.Count; i++)
                {
                    if (this.bpmChangeNotes[i].TickStamp <= overallTick)
                    {
                        maximumBPMIndex = i;
                    }
                }
                if (maximumBPMIndex == 0)
                {
                    result = GetBPMTimeUnit(this.bpmChangeNotes[0].BPM) * overallTick;
                }
                else
                {
                    for (int i = 1; i <= maximumBPMIndex; i++)
                    {
                        double previousTickTimeUnit = GetBPMTimeUnit(this.bpmChangeNotes[i - 1].BPM);
                        result += (this.bpmChangeNotes[i].TickStamp - this.bpmChangeNotes[i - 1].TickStamp) * previousTickTimeUnit;
                    }
                    double tickTimeUnit = GetBPMTimeUnit(this.bpmChangeNotes[maximumBPMIndex].BPM);
                    result += (overallTick - this.bpmChangeNotes[maximumBPMIndex].TickStamp) * tickTimeUnit;
                }
            }
            return result;
        }
    }
}
