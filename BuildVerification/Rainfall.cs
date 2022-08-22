using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildVerification
{
    class Rainfall
    {
        public DownloadRequest DownloadRequest { get; set; }

        /// <summary>
        /// Parses how much rain has fallen within the last few months for a specific station.
        /// </summary>
        /// <param name="station">Station to parse from</param>
        /// <param name="version">Version of API</param>
        /// <param name="period">Period of time to calculate rainfall for</param>
        public void Parse(string station, string version, string period)
        {
            DownloadRequest = new DownloadRequest();
            var nsmgr = new XmlNamespaceManager(DownloadRequest.Document.NameTable);
            nsmgr.AddNamespace("metObsIntervalData", "https://opendata.smhi.se/xsd/metobs_v1.xsd");
            DownloadRequest.GetData("https://opendata-download-metobs.smhi.se/api/version/" + version + "/parameter/23/station/" + station + "/period/" + period + "/data.xml");

            if (DownloadRequest.Document == null) // If document is null, throw exception.
                throw new Exception("Document is null");

            var values = DownloadRequest.Document.DocumentElement.SelectNodes("//metObsIntervalData:value/metObsIntervalData:value", nsmgr);

            var totalRain = 0.0f;
            foreach (XmlNode val in values)
            {
                totalRain += float.Parse(val.InnerText);
            }

            Console.WriteLine(GetPeriod() + " the total rainfall in " + GetName() + " was " + totalRain + " millimeters");
        }

        /// <summary>
        /// Gets the period of time for rainfall.
        /// </summary>
        /// <returns>Formatted string that contains rainfall</returns>
        private string GetPeriod()
        {
            XmlNodeList period = DownloadRequest.Document.GetElementsByTagName("period");
            string[] timeArray = new string[2];
            timeArray[0] = period[0]["from"]?.InnerText;
            timeArray[1] = period[0]["to"]?.InnerText;
            timeArray[0] = StringExtension.Truncate(timeArray[0], 10);
            timeArray[1] = StringExtension.Truncate(timeArray[1], 10);

            return "Between " + timeArray[0] + " and " + timeArray[1];
        }

        /// <summary>
        /// Gets the name of the location
        /// </summary>
        /// <returns>Location name</returns>
        private string GetName()
        {
            XmlNodeList station = DownloadRequest.Document.GetElementsByTagName("station");
            return station[0]["name"].InnerText;
        }
    }
}
