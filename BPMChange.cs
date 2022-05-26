namespace MaichartConverter
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
            this.Tick = 0;
            this.BPM = 0;
            this.Update();
        }

        /// <summary>
        /// Construct BPMChange with given bar, tick, BPM
        /// </summary>
        /// <param name="bar">Bar</param>
        /// <param name="startTime">tick</param>
        /// <param name="BPM">BPM</param>
        public BPMChange(int bar, int startTime, double BPM)
        {
            this.Bar = bar;
            this.Tick = startTime;
            this.BPM = BPM;
            this.Update();
        }

        /// <summary>
        /// Construct BPMChange with take in value
        /// </summary>
        /// <param name="takeIn">Take in BPMChange</param>
        public BPMChange(BPMChange takeIn)
        {
            this.Bar = takeIn.Bar;
            this.Tick = takeIn.Tick;
            this.BPM = takeIn.BPM;
            this.Update();
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
                //result += "(" + this.BPM + "_" + this.Bar + "_" + this.Tick + ")";
            }
            else result += "(" + this.BPM + "_" + this.Bar + "_" + this.Tick + ")";
            return result;
        }

        public override string NoteGenre => "BPM";

        public override bool IsNote => true;

        public override string NoteSpecificType => "BPM";

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (this == obj && this == null)
            {
                result = true;
            }
            else if (this!=null && obj != null)
            {
                BPMChange candidate = (BPMChange)obj;
                if (this.GetHashCode() == candidate.GetHashCode())
                {
                    result = true;
                }
                else if (this.Bar == candidate.Bar)
                {
                    if (this.Tick == candidate.Tick && this.BPM == candidate.BPM)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            // string hash = this.Bar + "0" + this.Tick + "0" + this.BPM;
            // return int.Parse(hash);
            return base.GetHashCode();
        }
    }
}
