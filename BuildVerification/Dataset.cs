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

        /// <summary>
        /// Loads a file from a url.
        /// </summary>
        /// <param name="url">URL to load from</param>
        /// <param name="forceReload">Forces the instance to reload the file</param>
        /// <returns>The loaded XML document</returns>
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

            // Checks if we're loading a station-set or not.
            // If we are, we need to first enter the station, then loop through all the values
            // If not, we just need to loop through the values
            // this is to handle datasets that have multiple values, such as a period like "latest-months" in both situations where we're fetching from all stations and from individual stations.
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