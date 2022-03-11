namespace MaichartConverter
{
    /// <summary>
    /// Defines measure change note that indicates a measure change in bar.
    /// </summary>
    public class MeasureChange : Note
    {        
        private int quaver;

        /// <summary>
        /// Construct Empty
        /// </summary>
        public MeasureChange()
        {
            this.Tick = 0;
            this.quaver = 0;
        }

        /// <summary>
        /// Construct BPMChange with given bar, tick, BPM
        /// </summary>
        /// <param name="bar">Bar</param>
        /// <param name="tick">Tick</param>
        /// <param name="Quaver">Quaver</param>
        public MeasureChange(int bar, int tick, int quaver)
        {
            this.Bar = bar;
            this.Tick = tick;
            this.quaver = quaver;
        }
        
        /// <summary>
        /// Construct measureChange from another takeIn
        /// </summary>
        /// <param name="takeIn">Another measure change note</param>
        public MeasureChange(MeasureChange takeIn)
        {
            this.Bar = takeIn.Bar;
            this.Tick = takeIn.Tick;
            this.quaver = takeIn.Quaver;
        }

        /// <summary>
        /// Return this.quaver
        /// </summary>
        /// <value>Quaver</value>
        public int Quaver
        {
            get { return this.quaver; }
        }

        public override bool CheckValidity()
        {
            return this.quaver>0;
        }

        public override string Compose(int format)
        {
            string result = "";
            if (format == 0)
            {
                result += "{" + this.Quaver + "}";
                //result += "{" + this.Quaver+"_"+this.Tick + "}";
            }
            return result;
        }

        public override string NoteGenre => "MEASURE";

        public override bool IsNote => false;

        public override string NoteSpecificType => "MEASURE";
    }
}
