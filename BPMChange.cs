namespace MaidataConverter
{
    /// <summary>
    /// BPMChange note for Simai
    /// </summary>
    public class BPMChange : Note
    {

        /// <summary>
        /// Construct Empty
        /// </summary>
        public BPMChange()
        {
            this.NoteType = "BPM";
            this.Key = "";
            this.Bar = 0;
            this.StartTime = 0;
            this.BPM = 0;
        }

        /// <summary>
        /// Construct BPMChange with given bar, tick, BPM
        /// </summary>
        /// <param name="bar">Bar</param>
        /// <param name="startTime">startTime</param>
        /// <param name="BPM">BPM</param>
        public BPMChange(int bar, int startTime, double BPM)
        {
            this.Bar = bar;
            this.StartTime = startTime;
            this.BPM = BPM;
        }

        /// <summary>
        /// Construct BPMChange with takein value
        /// </summary>
        /// <param name="takein">Take in BPMChange</param>
        public BPMChange(BPMChange takein)
        {
            this.Bar = takein.Bar;
            this.StartTime = takein.StartTime;
            this.BPM = takein.BPM;
        }


        public override bool CheckValidity()
        {
            return this.BPM != 0;
        }

        public override string Compose(int format)
        {
            string result = "";
            if (format == 0)
            {
                result += "(" + this.BPM + ")";
                // result += "(" + this.BPM + "_" + this.Bar + "_" + this.StartTime + ")";
            }
            else result += "(" + this.BPM + "_" + this.Bar + "_" + this.StartTime + ")";
            return result;
        }

        public override string NoteGenre()
        {
            return "BPM";
        }

        public override bool IsNote()
        {
            return true;
        }

        public override string NoteSpecificType()
        {
            return "BPM";
        }
    }
}
