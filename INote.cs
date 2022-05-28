namespace MaichartConverter
{
    /// <summary>
    /// Provide interface and basic functions for Notes
    /// </summary>
    interface INote
    {
        /// <summary>
        /// Convert note to string for writing
        /// </summary>
        /// <param name="format">0 if maiData, 1 if GoodBrother</param>
        string Compose(int format);

        /// <summary>
        /// See if current note has all information needed
        /// </summary>
        /// <returns>True if qualified, false elsewise</returns>
        bool CheckValidity();

        /// <summary>
        /// Updates this note instance.
        /// </summary>
        /// <returns>True if Calculated Times is defined, false elsewise</returns>
        bool Update();

        /// <summary>
        /// Give time stamp given overall tick
        /// </summary>
        /// <param name="overallTick">Note.Bar*384+Note.Tick</param>
        /// <returns>Appropriate time stamp in seconds</returns>
        double GetTimeStamp(int overallTick);
    }
}
