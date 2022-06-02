namespace MaichartConverter
{
    /// <summary>
    /// Construct Rest Note solely for Simai
    /// </summary>
    internal class Rest : Note
    {

        /// <summary>
        /// Construct empty
        /// </summary>
        public Rest()
        {
            this.NoteType = "RST";
            this.Bar = 0;
            this.Tick = 0;
            this.Update();
        }

        /// <summary>
        /// Construct Rest Note with given information
        /// </summary>
        /// <param name="noteType">Note Type to take in</param>
        /// <param name="bar">Bar to take in</param>
        /// <param name="startTime">Start to take in</param>
        public Rest(string noteType, int bar, int startTime)
        {
            this.NoteType = noteType;
            this.Bar = bar;
            this.Tick = startTime;
            this.Update();
        }

        /// <summary>
        /// Construct with Note provided
        /// </summary>
        /// <param name="n">Note to take in</param>
        public Rest(Note n)
        {
            this.NoteType = "RST";
            this.Bar = n.Bar;
            this.Tick = n.Tick;
            this.BPMChangeNotes = n.BPMChangeNotes;
            this.Update();
        }
        public override bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public override string Compose(int format)
        {
            //return "r_" + this.Tick;
            return "";
        }

        public override string NoteGenre => "REST";
        public override bool IsNote => false;

        public override string NoteSpecificType => "REST";
    }
}
