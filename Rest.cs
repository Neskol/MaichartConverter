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
            this.StartTime = 0;
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
            this.StartTime = startTime;
        }

        /// <summary>
        /// Construct with Note provided
        /// </summary>
        /// <param name="n">Note to take in</param>
        public Rest(Note n)
        {
            this.NoteType = "RST";
            this.Bar = n.Bar;
            this.StartTime = n.StartTime;
        }
        public override bool CheckValidity()
        {
            throw new NotImplementedException();
        }

        public override string Compose(int format)
        {
            // return "r_" + this.StartTime;
            return "";
        }

        public override string NoteGenre()
        {
            return "REST";
        }
        public override bool IsNote()
        {
            return false;
        }

        public override string NoteSpecificType()
        {
            return "REST";
        }
    }
}
