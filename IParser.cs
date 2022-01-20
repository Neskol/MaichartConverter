namespace MaidataConverter
{
    public interface IParser
    {
        /// <summary>
        /// Return correct GoodBrother of given Token.
        /// </summary>
        /// <param name="token">Token to intake</param>
        /// <returns>Corresponding GoodBrother</returns>
        public GoodBrother1 GoodBrotherOfToken(string[] token);

        /// <summary>
        /// Return correct BPMChanges of given Token.
        /// </summary>
        /// <param name="token">Token to intake</param>
        /// <returns>Corresponding BPMChanges</returns>
        public BPMChanges BPMChangesOfToken(string token);

        /// <summary>
        /// Return corresponding MeasureChanges
        /// </summary>
        /// <param name="token">Intake token</param>
        /// <returns>Corresponding measure change</returns>
        public MeasureChanges MeasureChangesOfToken(string token);

        /// <summary>
        /// Return a specific note of given Token.
        /// </summary>
        /// <param name="token">Token to take in</param>
        /// <returns>Specific Note</returns>
        public Note NoteOfToken(string token);

        /// <summary>
        /// Return correct Tap note.
        /// </summary>
        /// <param name="token">Token to take in</param>
        /// <returns>Specific Tap</returns>
        public Tap TapOfToken(string token);

        /// <summary>
        /// Return correct Hold note.
        /// </summary>
        /// <param name="token">Token to take in</param>
        /// <returns>Specific Hold Note</returns>
        public Hold HoldOfToken(string token);

        /// <summary>
        /// Return correct Slide note.
        /// </summary>
        /// <param name="token">Token to take in</param>
        /// <returns>Specific Slide Note</returns>
        public Slide SlideOfToken(string token);
    }
}

