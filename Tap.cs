namespace MaichartConverter
{
    /// <summary>
    /// Tap note
    /// </summary>
    public class Tap : Note
    {
        /// <summary>
        /// Stores if the Touch note have special effect
        /// </summary>
        private int specialEffect;

        /// <summary>
        /// Stores how big the note is: M1 for Regular and L1 for large
        /// </summary>
        private string touchSize;

        /// <summary>
        /// Stores enums of accepting tap notes
        /// </summary>
        /// <value></value>
        private readonly string[] allowedType = { "TAP", "STR", "BRK", "BST", "XTP", "XST", "TTP", "NST", "NSS" };

        /// <summary>
        /// Empty Constructor Tap Note
        /// </summary>
        public Tap()
        {
            this.touchSize="M1";
            this.Update();
        }

        /// <summary>
        /// Construct a Tap note
        /// </summary>
        /// <param name="noteType">TAP,STR,BRK,BST,XTP,XST,TTP; NST or NSS</param>
        /// <param name="key">0-7 representing each key</param>
        /// <param name="bar">Bar location</param>
        /// <param name="startTime">Start Location</param>
        public Tap(string noteType, int bar, int startTime, string key)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.specialEffect = 0;
            this.touchSize = "M1";
            this.Update();
        }

        /// <summary>
        /// Construct a Touch note with parameter taken in
        /// </summary>
        /// <param name="noteType">"TTP"</param>
        /// <param name="bar">Bar location</param>
        /// <param name="startTime">Start Location</param>
        /// <param name="key">Key</param>
        /// <param name="specialEffect">Effect after touch</param>
        /// <param name="touchSize">L=larger notes M=Regular</param>
        public Tap(string noteType, int bar, int startTime, string key, int specialEffect, string touchSize)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.specialEffect = specialEffect;
            this.touchSize = touchSize;
            this.Update();
        }
        
        /// <summary>
        /// Construct a Tap note form another note
        /// </summary>
        /// <param name="inTake">The intake note</param>
        /// <exception cref="NullReferenceException">Will raise exception if touch size is null</exception>
        public Tap(Note inTake)
        {
            this.NoteType = inTake.NoteType;
            this.Key = inTake.Key;
            this.EndKey = inTake.EndKey;
            this.Bar = inTake.Bar;
            this.Tick = inTake.Tick;
            this.TickStamp = inTake.TickStamp;
            this.TickTimeStamp = inTake.TickTimeStamp;
            this.LastLength = inTake.LastLength;
            this.LastTickStamp = inTake.LastTickStamp;
            this.LastTimeStamp = inTake.LastTimeStamp;
            this.WaitLength = inTake.WaitLength;
            this.WaitTickStamp = inTake.WaitTickStamp;
            this.WaitTimeStamp = inTake.WaitTimeStamp;
            this.CalculatedLastTime = inTake.CalculatedLastTime;
            this.CalculatedLastTime = inTake.CalculatedLastTime;
            this.TickBPMDisagree = inTake.TickBPMDisagree;
            this.BPM = inTake.BPM;
            this.BPMChangeNotes = inTake.BPMChangeNotes;
            if (inTake.NoteGenre == "TAP")
            {
                this.touchSize = ((Tap)inTake).TouchSize ?? throw new NullReferenceException();
                this.SpecialEffect = ((Tap)inTake).SpecialEffect;
            }
            else
            {
                this.touchSize = "M1";
                this.SpecialEffect = 0;
            }
        }


        /// <summary>
        /// Return this.specialEffect
        /// </summary>
        public int SpecialEffect
        {
            get { return this.specialEffect; }
            set { this.specialEffect = value; }
        }

        /// <summary>
        /// Return this.touchSize
        /// </summary>
        public string TouchSize
        {
            get { return this.touchSize; }
            set {  this.touchSize = value; }
        }

        public override bool CheckValidity()
        {
            bool result = false;
            foreach (string x in allowedType)
            {
                result = result || this.NoteType.Equals(x);
            }
            result = result && NoteType.Length == 3;
            result = result && Key.Length <= 2;
            return result;
        }

        public override string Compose(int format)
        {
            string result = "";
            if (format == 1 && !(this.NoteType.Equals("TTP")) && !((this.NoteType.Equals("NST"))||this.NoteType.Equals("NSS")))
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key;
            }
            else if (format == 1 && (this.NoteType.Equals("NST")||this.NoteType.Equals("NSS")))
            {
                result = ""; //NST and NSS is just a place holder for slide
            }
            else if (format == 1 && this.NoteType.Equals("TTP"))
            {
                result = this.NoteType + "\t" +
                    this.Bar + "\t" +
                    this.Tick + "\t" +
                    this.Key.ToCharArray()[1] + "\t" +
                    this.Key.ToCharArray()[0] + "\t" +
                    this.specialEffect + "\t" +
                    this.touchSize; //M1 for regular note and L1 for Larger Note
            }
            else if (format == 0)
            {
                switch (this.NoteType)
                {
                    case "TAP":
                        result += (Int32.Parse(this.Key) + 1).ToString();
                        break;
                    case "STR":
                        result += (Int32.Parse(this.Key) + 1).ToString();
                        break;
                    case "BRK":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "b";
                        break;
                    case "BST":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "b";
                        break;
                    case "XTP":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "x";
                        break;
                    case "XST":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "x";
                        break;
                    case "NST":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "!";
                        break;
                    case "NSS":
                        result += (Int32.Parse(this.Key) + 1).ToString() + "$";
                        break;
                    case "TTP":
                        result += this.Key.ToCharArray()[1] + ((Convert.ToInt32(this.Key.Substring(0, 1)) + 1).ToString());
                        if (this.SpecialEffect == 1)
                        {
                            result += "f";
                        }
                        break;
                }
                //result += "_" + this.Tick;
            }
            return result;
        }

        public override string NoteGenre => "TAP";

        public override bool IsNote
        {
            get
            {
                if (this.NoteType.Equals("NST"))
                {
                    return false;
                }
                else return true;
            }
        }

        public override string NoteSpecificType
        {
            get
            {
                string result = "";
                switch (this.NoteType)
                {
                    case "TAP":
                        result += "TAP";
                        break;
                    case "STR":
                        result += "SLIDE_START";
                        break;
                    case "BRK":
                        result += "TAP";
                        break;
                    case "BST":
                        result += "SLIDE_START";
                        break;
                    case "XTP":
                        result += "TAP";
                        break;
                    case "XST":
                        result += "SLIDE_START";
                        break;
                    case "TTP":
                        result += "TAP";
                        break;
                    case "NST":
                        result += "SLIDE_START";
                        break;
                    case "NSS":
                        result += "SLIDE_START";
                        break;
                }

                return result;
            }
        }
    }
}


