using System;
using System.Xml;

namespace MaichartConverter
{
    /// <summary>
    /// Using xml to store maicharts
    /// </summary>
    public class XMaiL : Chart, ICompiler
    {
        /// <summary>
        /// Storage of Xml file
        /// </summary>
        private XmlDocument StoredXMailL;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMaiL()
        {
            this.Notes = new List<Note>();
            this.BPMChanges = new BPMChanges();
            this.MeasureChanges = new MeasureChanges();
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.StoredXMailL = new XmlDocument();
            this.Update();
        }

        /// <summary>
        /// Construct XMaiL with given notes, bpm change definitions and measure change definitions.
        /// </summary>
        /// <param name="notes">Notes in XMaiL</param>
        /// <param name="bpmChanges">BPM Changes: Initial BPM is NEEDED!</param>
        /// <param name="measureChanges">Measure Changes: Initial Measure is NEEDED!</param>
        public XMaiL(List<Note> notes, BPMChanges bpmChanges, MeasureChanges measureChanges)
        {
            this.Notes = notes;
            this.BPMChanges = bpmChanges;
            this.MeasureChanges = measureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.StoredXMailL = new XmlDocument();
            this.Update();
        }

        /// <summary>
        /// Construct XMaiL with tokens given
        /// </summary>
        /// <param name="tokens">Tokens given</param>
        public XMaiL(string[] tokens)
        {
            Chart takenIn = new Ma2Parser().ChartOfToken(tokens);
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.StoredXMailL = new XmlDocument();
            this.Update();
        }

        /// <summary>
        /// Construct XMaiL with existing values
        /// </summary>
        /// <param name="takenIn">Existing good brother</param>
        public XMaiL(Chart takenIn)
        {
            this.Notes = takenIn.Notes;
            this.BPMChanges = takenIn.BPMChanges;
            this.MeasureChanges = takenIn.MeasureChanges;
            this.StoredChart = new List<List<Note>>();
            this.Information = new Dictionary<string, string>();
            this.StoredXMailL = new XmlDocument();
            this.Update();
        }
        
        public override string Compose()
        {
            XmlDeclaration xmlDecl = this.StoredXMailL.CreateXmlDeclaration("1.0","UTF-8",null);
            this.StoredXMailL.AppendChild(xmlDecl);
            XmlElement root = this.StoredXMailL.CreateElement("XMaiL");
            XmlAttribute rootVersion = this.StoredXMailL.CreateAttribute("1.0");
            root.Attributes.Append(rootVersion);
            this.StoredXMailL.AppendChild(root);
            XmlElement information = this.StoredXMailL.CreateElement("TrackInformation");
            throw new NotImplementedException();
        }

        public override string Compose(BPMChanges bpm, MeasureChanges measure)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            
        }
    }
}