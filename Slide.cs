namespace MaichartConverter
{
    /// <summary>
    /// Construct a Slide note (With START!)
    /// </summary>
    public class Slide : Note
    {
        private readonly string[] allowedType = { "SI_", "SV_", "SF_", "SCL", "SCR", "SUL", "SUR", "SLL", "SLR", "SXL", "SXR", "SSL", "SSR" };

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Slide()
        {
        }

        /// <summary>
        /// Construct a Slide Note (Valid only if Start Key matches a start!)
        /// </summary>
        /// <param name="noteType">SI_(Straight),SCL,SCR,SV_(Line not intercepting Crossing Center),SUL,SUR,SF_(Wifi),SLL(Infecting Line),SLR(Infecting),SXL(Self winding),SXR(Self winding),SSL,SSR</param>
        /// <param name="key">0-7</param>
        /// <param name="bar">Bar in</param>
        /// <param name="startTime">Start Time</param>
        /// <param name="lastTime">Last Time</param>
        /// <param name="endKey">0-7</param>
        public Slide(string noteType, int bar, int startTime, string key, int waitTime, int lastTime, string endKey)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.WaitLength = waitTime;
            this.LastLength = lastTime;
            this.EndKey = endKey;
            this.Delayed = this.WaitLength != 96;
            this.Update();
        }

        /// <summary>
        /// Construct a Slide from another note
        /// </summary>
        /// <param name="inTake">The intake note</param>
        public Slide(Note inTake)
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
            if (format == 1)
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key + "\t" + this.WaitLength + "\t" + this.LastLength + "\t" + this.EndKey;
            }
            else if (format == 0)
            {
                switch (this.NoteType)
                {
                    case "SI_":
                        result += "-";
                        break;
                    case "SV_":
                        result += "v";
                        break;
                    case "SF_":
                        result += "w";
                        break;
                    case "SCL":
                        if (Int32.Parse(this.Key) == 0 || Int32.Parse(this.Key) == 1 || Int32.Parse(this.Key) == 6 || Int32.Parse(this.Key) == 7)
                        {
                            result += "<";
                        }
                        else
                            result += ">";
                        break;
                    case "SCR":
                        if (Int32.Parse(this.Key) == 0 || Int32.Parse(this.Key) == 1 || Int32.Parse(this.Key) == 6 || Int32.Parse(this.Key) == 7)
                        {
                            result += ">";
                        }
                        else
                            result += "<";
                        break;
                    case "SUL":
                        result += "p";
                        break;
                    case "SUR":
                        result += "q";
                        break;
                    case "SSL":
                        result += "s";
                        break;
                    case "SSR":
                        result += "z";
                        break;
                    case "SLL":
                        result += "V" + GenerateInflection(this);
                        break;
                    case "SLR":
                        result += "V" + GenerateInflection(this);
                        break;
                    case "SXL":
                        result += "pp";
                        break;
                    case "SXR":
                        result += "qq";
                        break;
                }
                if (this.TickBPMDisagree || this.Delayed)
                {
                    result += ((Convert.ToInt32(this.EndKey) + 1).ToString()) + GenerateAppropriateLength(this.LastLength, this.BPM);
                }
                else
                {
                    result += ((Convert.ToInt32(this.EndKey) + 1).ToString()) + GenerateAppropriateLength(this.LastLength);
                }
                //result += "_" + this.Tick;
                //result += "_" + this.Key;
            }
            return result;
        }

        /// <summary>
        /// Return inflection point of SLL and SLR
        /// </summary>
        /// <param name="x">This note</param>
        /// <returns>Infection point of this note</returns>
        public static int GenerateInflection(Note x)
        {
            int result = Int32.Parse(x.Key) + 1;
            if (x.NoteType.Equals("SLR"))
            {
                result += 2;
            }
            else if (x.NoteType.Equals("SLL"))
            {
                result -= 2;
            }

            if (result > 8)
            {
                result -= 8;
            }
            else if (result < 1)
            {
                result += 8;
            }

            if (result == Int32.Parse(x.Key) + 1 || (result == Int32.Parse(x.EndKey) + 1))
            {
                //Deal with result;
                if (result>4)
                {
                    result -= 4;
                }
                else if (result<=4)
                {
                    result += 4;
                }

                //Deal with note type;
                if (x.NoteType.Equals("SLL"))
                {
                    x.NoteType = "SLR";
                }
                else if (x.NoteType.Equals("SLR"))
                {
                    x.NoteType = "SLL";
                }
                else
                {
                    throw new InvalidDataException("INFLECTION POINT IS THE SAME WITH ONE OF THE KEY!");
                }
            }

            return result;
        }

        public override string NoteGenre => "SLIDE";

        public override bool IsNote => true;

        public override string NoteSpecificType => "SLIDE";

    }
}
