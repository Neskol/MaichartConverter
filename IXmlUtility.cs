using System.Xml;

namespace MaichartConverter
{
    /// <summary>
    /// Provide handful methods for Xml
    /// </summary>
    internal interface IXmlUtility
    {
        ///// <summary>
        ///// Load and construct Xml document from given location.
        ///// </summary>
        ///// <param name="location">Location to find</param>
        //public void Load(string location);

        /// <summary>
        /// Save Xml to specified location.
        /// </summary>
        /// <param name="location">Location to save</param>
        public void Save(string location);

        /// <summary>
        /// Return nodes with specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>XmlNodeList having name specified</returns>
        public XmlNodeList GetMatchNodes(string name);

        /// <summary>
        /// Update the information using given takeinValue.
        /// </summary>
        public void Update();
    }
}
