namespace MaichartConverter
{
    public class MeasureChange : Note
    {
        private int bar;
        private int tick;
        private int quaver;

        /// <summary>
        /// Construct Empty
        /// </summary>
        public MeasureChange()
        {
            this.bar = 0;
            this.tick = 0;
            this.quaver = 0;
        }

        /// <summary>
        /// Construct BPMChange with given bar, tick, BPM
        /// </summary>
        /// <param name="bar">Bar</param>
        /// <param name="tick">Tick</param>
        /// <param name="BPM">BPM</param>
        public MeasureChange(int bar, int tick, int quaver)
        {
            this.bar = bar;
            this.tick = tick;
            this.quaver = quaver;
        }

        public MeasureChange(MeasureChange takein)
        {
            this.bar = takein.Bar;
            this.tick = takein.Tick;
            this.quaver = takein.Quaver;
        }

        public int Tick
        {
            get
            { return this.tick; }
        }

        public int Quaver
        {
            get { return this.quaver; }
        }
        public override bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public override string Compose(int format)
        {
            string result = "";
            if (format == 0)
            {
                result += "{" + this.Quaver + "}";
                //result += "{" + this.Quaver+"_"+this.StartTime + "}";
            }
            return result;
        }

        public override string NoteGenre => "MEASURE";

        public override bool IsNote => false;

        public override string NoteSpecificType => "MEASURE";
    }
}
