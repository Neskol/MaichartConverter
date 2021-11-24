using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicConverterTest
{
    internal class XmlUtility : IXmlUtility
    {
        private Dictionary<string, string> information;
        private XmlDocument takeinValue;

        public XmlUtility()
        {
            takeinValue = new XmlDocument();
            information = new Dictionary<string, string>();
        }

        public XmlUtility(string location)
        {
            this.takeinValue = new XmlDocument();
            this.takeinValue.LoadXml(location);
            this.information = new Dictionary<string, string>();
        }

        public XmlNodeList GetMatchNodes(string name)
        {
            XmlNodeList result = this.takeinValue.GetElementsByTagName(name);
            return result;
        }

        public XmlDocument TakeinValue
        {
            get { return takeinValue; }
        }

        public Dictionary<string, string> Information
        {
            get { return information; }
        }

        public void Load(string location)
        {
            this.takeinValue = new XmlDocument();
            this.takeinValue.LoadXml(location);
            this.information = new Dictionary<string, string>();
        }

        public void Save(string location)
        {
            this.takeinValue.Save(location);
        }

        public void Update()
        {
            try
            {
                XmlNodeList nameCandidate = takeinValue.GetElementsByTagName("name");
                XmlNodeList bpmCandidate = takeinValue.GetElementsByTagName("bpm");
                XmlNodeList chartCandidate = takeinValue.GetElementsByTagName("Notes");
                XmlNodeList composerCandidate = takeinValue.GetElementsByTagName("artistName");
                XmlNodeList genreCandidate = takeinValue.GetElementsByTagName("genreName");
                //Add in name and music ID.
                ////Add BPM
                //this.information.Add("BPM",bpmCandidate[0].InnerText);
                foreach (XmlNode candidate in nameCandidate)
                {
                    if (!this.information.ContainsKey("Music ID"))
                    {
                        this.information.Add("Music ID", candidate["id"].InnerText);
                        this.information.Add("Name", candidate["name"].InnerText);
                    }
                }
                //Add Chart information
                //{
                //    //Add Basic Chart
                //    XmlNode basicCandidate = chartCandidate[0];
                //    string basicChartPath = basicCandidate["file"].InnerText;
                    
                //}
                foreach (XmlNode candidate in chartCandidate)
                {
                    if (candidate["file"]["path"].InnerText.Contains(this.information.GetValueOrDefault("00.ma2")))
                    {
                        this.information.Add("Basic", candidate["musicLevelID"].InnerText);
                        this.information.Add("Basic Chart Maker", candidate["noteDesigner"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains(this.information.GetValueOrDefault("01.ma2")))
                    {
                        this.information.Add("Advanced", candidate["musicLevelID"].InnerText);
                        this.information.Add("Advanced Chart Maker", candidate["noteDesigner"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains(this.information.GetValueOrDefault("02.ma2")))
                    {
                        this.information.Add("Expert", candidate["musicLevelID"].InnerText);
                        this.information.Add("Expert Chart Maker", candidate["noteDesigner"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains(this.information.GetValueOrDefault("03.ma2")))
                    {
                        this.information.Add("Master", candidate["musicLevelID"].InnerText);
                        this.information.Add("Master Chart Maker", candidate["noteDesigner"].InnerText);
                    }
                    else if (candidate["file"]["path"].InnerText.Contains(this.information.GetValueOrDefault("04.ma2")))
                    {
                        this.information.Add("Remaster", candidate["musicLevelID"].InnerText);
                        this.information.Add("Remaster Chart Maker", candidate["noteDesigner"].InnerText);
                    }
                }

            }catch (Exception e)
            {

            }
        }
    }
}
