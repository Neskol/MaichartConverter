namespace MaichartConverter
{
    /// <summary>
    /// Constructs Hold Note
    /// </summary>
    public class Hold : Note
    {
        /// <summary>
        /// Stores if this Touch Hold have special effect
        /// </summary>
        private int specialEffect;

        /// <summary>
        /// Stores the size of touch hold
        /// </summary>
        private string touchSize;

        /// <summary>
        /// Stores enums of accepting Hold type
        /// </summary>
        /// <value></value>
        private readonly string[] allowedType = { "HLD", "XHO", "THO" };

        /// <summary>
        /// Construct a Hold Note
        /// </summary>
        /// <param name="noteType">HLD,XHO</param>
        /// <param name="key">Key of the hold note</param>
        /// <param name="bar">Bar of the hold note</param>
        /// <param name="startTime">Tick of the hold note</param>
        /// <param name="lastTime">Last time of the hold note</param>
        public Hold(string noteType, int bar, int startTime, string key, int lastTime)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.LastLength = lastTime;
            this.specialEffect = 0;
            this.touchSize = "M1";
            this.Update();
        }

        /// <summary>
        /// Construct a Touch Hold Note
        /// </summary>
        /// <param name="noteType">THO</param>
        /// <param name="key">Key of the hold note</param>
        /// <param name="bar">Bar of the hold note</param>
        /// <param name="startTime">Tick of the hold note</param>
        /// <param name="lastTime">Last time of the hold note</param>
        /// <param name = "specialEffect">Store if the touch note ends with special effect</param>
        /// <param name = "touchSize">Determines how large the touch note is</param>
        public Hold(string noteType, int bar, int startTime, string key, int lastTime, int specialEffect, string touchSize)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.Tick = startTime;
            this.LastLength = lastTime;
            this.specialEffect = specialEffect;
            this.touchSize = "touchSize";
            this.Update();
        }

        /// <summary>
        /// Returns if the note comes with Special Effect
        /// </summary>
        /// <value>0 if no, 1 if yes</value>
        public int SpecialEffect
        {
            get { return specialEffect; }
        }

        /// <summary>
        /// Returns the size of the note
        /// </summary>
        /// <value>M1 if regular, L1 if large</value>
        public string TouchSize
        {
            get { return touchSize; }
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
            if (format == 1 && !(this.NoteType.Equals("THO")))
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key + "\t" + this.LastLength;
            }
            else if (format == 1 && this.NoteType.Equals("THO"))
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.Tick + "\t" + this.Key.ToCharArray()[0] + "\t" + this.LastLength + "\t" + this.Key.ToCharArray()[1] + "\t"+this.SpecialEffect+"\tM1"; //M1 for regular note and L1 for Larger Note
            }
            else if (format == 0)
            {
                switch (this.NoteType)
                {
                    case "HLD":
                        result += (Convert.ToInt32(this.Key) + 1) + "h" + GenerateAppropriateLength(this.LastLength);
                        break;
                    case "XHO":
                        result += (Convert.ToInt32(this.Key) + 1) + "xh" + GenerateAppropriateLength(this.LastLength);
                        break;
                    case "THO":
                        if (this.SpecialEffect == 1)
                        {
                            result += this.Key.ToCharArray()[1].ToString() + ((Convert.ToInt32(this.Key.Substring(0, 1)) + 1).ToString() + "hf" + GenerateAppropriateLength(this.LastLength));
                        }
                        else
                            result += this.Key.ToCharArray()[1].ToString() + ((Convert.ToInt32(this.Key.Substring(0, 1)) + 1).ToString() + "xh" + GenerateAppropriateLength(this.LastLength));
                        break;
                }
            }
            return result;
        }

        public override string NoteGenre => "HOLD";

        public override bool IsNote => true;

        public override string NoteSpecificType
        {
            get
            {
                string result = "HOLD";
                //switch (this.NoteType)
                //{
                //    case "HLD":
                //        result += "HOLD";
                //        break;
                //    case "XHO":
                //        result += "HOLD";
                //        break;
                //    case "THO":
                //        result += "HOLD_TOUCH";
                //        break;
                //}
                return result;
            }
        }
    }
}
