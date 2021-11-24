using System;
using System.Collections.Generic;
using System.Text;

namespace MusicConverterTest
{
    public class Hold : Note
    {
        private int specialEffect;
        private string touchSize;
        private readonly string[] allowedType = { "HLD", "XHO", "THO" };
        /// <summary>
        /// Construct a Hold Note
        /// </summary>
        /// <param name="noteType">HLD,XHO,THO</param>
        /// <param name="key"></param>
        /// <param name="bar"></param>
        /// <param name="startTime"></param>
        /// <param name="lastTime"></param>
        public Hold(string noteType, int bar, int startTime, string key, int lastTime)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.StartTime = startTime;
            this.LastTime = lastTime;
            this.specialEffect = 0;
            this.touchSize = "M1";
        }

        public Hold(string noteType, int bar, int startTime, string key, int lastTime, int specialEffect, string touchSize)
        {
            this.NoteType = noteType;
            this.Key = key;
            this.Bar = bar;
            this.StartTime = startTime;
            this.LastTime = lastTime;
            this.specialEffect = specialEffect;
            this.touchSize = "touchSize";
        }

        public int SpecialEffect
        {
            get { return specialEffect; }
        }

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
                result = this.NoteType + "\t" + this.Bar + "\t" + this.StartTime + "\t" + this.Key + "\t" + this.LastTime;
            }
            else if (format == 1 && (this.NoteType.Equals("THO") || this.NoteType.Equals("XHO")))
            {
                result = this.NoteType + "\t" + this.Bar + "\t" + this.StartTime + "\t" + this.Key.ToCharArray()[1] + "\t" + this.LastTime + "\t" + this.Key.ToCharArray()[0] + "\t0\tM1"; //M1 for regular note and L1 for Larger Note
            }
            else if (format == 0)
            {
                switch (this.NoteType)
                {
                    case "HLD":
                        result += (Convert.ToInt32(this.Key)+1) + "h" + GenerateAppropriateLength(this.LastTime);
                        break;
                    case "XHO":
                        result += (Convert.ToInt32(this.Key)+1) + "xh" + GenerateAppropriateLength(this.LastTime);
                        break;
                    case "THO":
                        if (this.SpecialEffect==1)
                        {
                            result += this.Key.ToCharArray()[1].ToString() + ((Convert.ToInt32(this.Key.Substring(0, 1)) + 1).ToString() + "hf" + GenerateAppropriateLength(this.LastTime));
                        }
                        else
                        result += this.Key.ToCharArray()[1].ToString()+((Convert.ToInt32(this.Key.Substring(0, 1)) + 1).ToString()+"xh"+GenerateAppropriateLength(this.LastTime));
                        break;
                }
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
            result = "[" + quaver.ToString()+":" + beat.ToString()+"]";
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

        public override string NoteGenre()
        {
            return "HOLD";
        }

        public override bool IsNote()
        {
            return true;
        }

        public override string NoteSpecificGenre()
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
