using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildVerification
{
    class SearchExtension
    {
        static DownloadRequest download = new DownloadRequest();

        /// <summary>
        /// Searches for locations based on names
        /// </summary>
        /// <param name="name">Name of location</param>
        /// <returns>Id of the searched location</returns>
        public static string GetLocationByName(string version, string parameter, string name)
        {
            download.GetData("https://opendata-download-metobs.smhi.se/api/version/" + version + "/parameter/" + parameter + ".xml");
            
            foreach(XmlNode station in download.Document.GetElementsByTagName("station"))
            {
                if (station["name"].InnerText == name)
                    return station["id"].InnerText;
            }
            return "";
        }

    }
}
