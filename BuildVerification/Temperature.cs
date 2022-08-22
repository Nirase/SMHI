using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildVerification
{
    class Temperature
    {
		DownloadRequest DownloadRequest { get; set; }

		/// <summary>
		/// Fetches the average temperture throughout a station or stationset
		/// </summary>
		/// <param name="version">Version of API</param>
		/// <param name="stationset">station or station-set</param>
		/// <param name="stationId">Id of station</param>
		/// <param name="timeperiod">Timeperiod to fetch within</param>
		public double GetAverageTemperature(string version = "1.0", string stationset = "station-set", string stationId = "all", string timeperiod = "latest-hour")
        {
			var url = "https://opendata-download-metobs.smhi.se/api/version/" + version + "/parameter/1/" + stationset + "/" + stationId + "/period/" + timeperiod + "/data.xml";

			DownloadRequest = new DownloadRequest();

			DownloadRequest.GetData(url);

			if (DownloadRequest.Document == null)
				throw new Exception("Document is null (url not found?)");

			var nsmgr = new XmlNamespaceManager(DownloadRequest.Document.NameTable);
			nsmgr.AddNamespace("metObsIntervalData", "https://opendata.smhi.se/xsd/metobs_v1.xsd");

			var values = DownloadRequest.Document.DocumentElement.SelectNodes("//metObsIntervalData:value/metObsIntervalData:value", nsmgr);

			var temperature = 0.0f;
			try
			{
				foreach (XmlNode value in values)
				{
					temperature += float.Parse(value.InnerText);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw e;
			}

			return temperature / values.Count;
		}
	}
}

/*
 
 /// <summary>
		/// Gets the average air temperature.
		/// </summary>
		/// <param name="version">Which version of the api to use</param>
		/// <param name="stationset">Specification if data is from a station or station-set</param>
		/// <param name="stationId">Which stationID to get data from</param>
		/// <param name="timeperiod">Time period to return values from</param>
		/// <returns>Average temperature</returns>
		private static async Task<double> GetTemperature(string version = "1.0", string stationset = "station-set", string stationId = "all", string timeperiod = "latest-hour")
		{
			var url = "https://opendata-download-metobs.smhi.se/api/version/" + version + "/parameter/1/" + stationset + "/" + stationId + "/period/" + timeperiod + "/data.json";
			try
			{
				using (var httpClient = new HttpClient())
				{
					var json = await httpClient.GetStringAsync(url);
					var temp = JsonSerializer.Deserialize<Data>(json);
					var validStations = 0;
					var totalTemperature = 0.0f;
					foreach (var station in temp.station)
					{
						if (station.value == null)
							continue;

						validStations++;
						totalTemperature += float.Parse(station.value[0].value);
					}

					var averageTemp = totalTemperature / validStations;
					return averageTemp;

				}

			}
			catch (HttpRequestException e)
			{
				Console.WriteLine(e);
				return 0;
			}
		}*/