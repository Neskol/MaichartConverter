namespace MaichartConverter
{
    /// <summary>
    /// Provide interface for charts
    /// </summary>
    interface IChart
    {
        /// <summary>
        /// Updates all information
        /// </summary>
        void Update();

        /// <summary>
        /// Check if this chart is valid
        /// </summary>
        /// <returns></returns>
        bool CheckValidity();

        /// <summary>
        /// Export this Good Brother
        /// </summary>
        /// <returns></returns>
        string Compose();

        /// <summary>
        /// Get appropriate time stamp of given tick
        /// </summary>
        /// <returns>Time stamp of bar and note</returns>
        /// <requires>this.bpmChanges!=null</requires>
        double GetTimeStamp(int bar, int tick);
    }
}
