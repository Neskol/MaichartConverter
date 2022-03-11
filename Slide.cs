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
            this.WaitTime = waitTime;
            this.LastTime = lastTime;
            this.EndKey = endKey;
            this.Delayed = this.WaitTime != 96;
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
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key + "\t" + this.WaitTime + "\t" + this.LastTime + "\t" + this.EndKey;
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
                if (this.Delayed)
                {
                    result += ((Convert.ToInt32(this.EndKey) + 1).ToString()) + GenerateAppropriateLength(this.LastTime, this.BPM);
                }
                else
                {
                    result += ((Convert.ToInt32(this.EndKey) + 1).ToString()) + GenerateAppropriateLength(this.LastTime);
                }
                //result += "_" + this.Tick;
                //result += "_" + this.Key;
            }
            return result;
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
        /// Generate appropriate length for hold and slide.
        /// </summary>
        /// <param name="length">Last Time</param>
        /// <param name="bpm">BPM</param>
        /// <returns>[Definition:Length]=[Quaver:Beat]</returns>
        public string GenerateAppropriateLength(int length, double bpm)
        {
            string result = "";
            double tickTime = 60 / bpm * 4 / 384;
            double sustain = this.WaitTime * tickTime;
            double duration = this.LastTime * tickTime;
            result = "[" + sustain + "##" + duration + "]";
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
            return result;
        }

        public override string NoteGenre => "SLIDE";

        public override bool IsNote => true;

        public override string NoteSpecificType => "SLIDE";

    }
}
