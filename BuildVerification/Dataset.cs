using System;
using System.Collections.Generic;
using System.Xml;

namespace BuildVerification
{
    internal class Dataset
    {
        public XmlDocument Document { get; set; }
        public List<Data> Data { get; set; }
        public Dataset()
        {
            Document = new XmlDocument();
            Data = new List<Data>();
        }
        /// <summary>
        /// Saving these parameters in here is slightly inefficient if a lot of datasets are loaded at once, and more parameters are added, but it is a small cost for much more maintainable code, 
        /// similar to the decisions made regarding the Data class. 
        /// </summary>
        public string From { get; set; }
        public string To { get; set; }

        internal XmlDocument GetData(string url, bool forceReload = false)
        {
            bool stationSet = url.Contains("station-set");
            if(Document.Attributes != null && !forceReload)
                return Document;

            Document.Load(url);
            var nsmgr = new XmlNamespaceManager(Document.NameTable);
            nsmgr.AddNamespace("metObsIntervalData", "https://opendata.smhi.se/xsd/metobs_v1.xsd");


            if (Document == null)
                throw new Exception("Document failed to load.");

            From = Document.DocumentElement.SelectSingleNode("//metObsIntervalData:period/metObsIntervalData:from", nsmgr).InnerText;
            To = Document.DocumentElement.SelectSingleNode("//metObsIntervalData:period/metObsIntervalData:to", nsmgr).InnerText;

            if(stationSet)
            {
                var stations = Document.DocumentElement.GetElementsByTagName("station");

                foreach(XmlNode station in stations)
                {
                    var values = station.SelectNodes("metObsIntervalData:value/metObsIntervalData:value", nsmgr);
                    foreach(XmlNode val in values)
                    {
                        Data.Add(new Data(station["name"].InnerText, val.InnerText));
                    }
                }
            }
            else
            {
                var station = Document.DocumentElement.SelectSingleNode("metObsIntervalData:station/metObsIntervalData:name", nsmgr);
                var values = Document.DocumentElement.SelectNodes("metObsIntervalData:value/metObsIntervalData:value", nsmgr);
                foreach(XmlNode val in values)
                {
                    Data.Add(new Data(station.InnerText, val.InnerText));
                }
            }

            return Document;
        }
    }
}