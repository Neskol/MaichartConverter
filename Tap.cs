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
        private readonly string[] allowedType = { "TAP", "STR", "BRK", "BST", "XTP", "XST", "TTP", "NST" };

        /// <summary>
        /// Empty Constructor Tap Note
        /// </summary>
        public Tap()
        {
            this.touchSize="";
        }

        /// <summary>
        /// Construct a Tap note
        /// </summary>
        /// <param name="noteType">TAP,STR,BRK,BST,XTP,XST,TTP</param>
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
        }


        /// <summary>
        /// Return this.specialEffect
        /// </summary>
        public int SpecialEffect
        {
            get { return this.specialEffect; }
        }

        /// <summary>
        /// Return this.touchSize
        /// </summary>
        public string TouchSize
        {
            get { return this.touchSize; }
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
            if (format == 1 && !(this.NoteType.Equals("TTP")) && !(this.NoteType.Equals("NTP")))
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key;
            }
            else if (format == 1 && this.NoteType.Equals("NST"))
            {
                result = ""; //NST is just a place holder for slide
            }
            else if (format == 1 && this.NoteType.Equals("TTP"))
            {
                result = this.NoteType + "\t" +
                    this.Bar + "\t" +
                    this.Tick + "\t" +
                    this.Key.ToCharArray()[0] + "\t" +
                    this.Key.ToCharArray()[1] + "\t" +
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
                    case "NTP":
                        result += "SLIDE_START";
                        break;
                }

                return result;
            }
        }
    }
}


