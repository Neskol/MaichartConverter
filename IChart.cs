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
    }
}
