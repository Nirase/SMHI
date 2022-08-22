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
        public Dataset Dataset { get; set; }
        public Rainfall(string stationId)
        {
            Dataset = new Dataset();
            Dataset.GetData("https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/23/station/" + stationId + "/period/latest-months/data.xml");
        }
        public double GetTotalRain()
        {
            var totalRain = 0.0f;
            try
            {
                foreach (var val in Dataset.Data)
                {
                    totalRain += float.Parse(val.Value);
                }
                return totalRain;
            }
            catch(Exception e)
            {
                totalRain = 0;
                throw e;
            }
            
        }

        /// <summary>
        /// Gets the period of time for rainfall.
        /// </summary>
        /// <returns>Formatted string that contains period of rainfall</returns>
        public string[] GetPeriod()
        {
            string[] fromTo = new string[2];
            fromTo[0] = StringExtension.Truncate(Dataset.From, 10);
            fromTo[1] = StringExtension.Truncate(Dataset.To, 10);
            return fromTo;
        }

        /// <summary>
        /// Gets the name of the current location
        /// </summary>
        /// <returns>Location name</returns>
        public string GetName()
        {
            return Dataset.Data[0].StationName;
        }
    }
}
