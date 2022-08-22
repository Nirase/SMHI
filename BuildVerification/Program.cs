using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;

namespace BuildVerification
{
	internal class Program
	{

		public static async Task Main(string[] args)
		{
			float totalRain = await GetTotalRain("1.0", "23", "station", "53430", "latest-months");
			string[] period = await GetPeriod("1.0", "23", "station", "53430", "latest-months");
			period[0] = Truncate(period[0], 10);
			period[1] = Truncate(period[1], 10);
			Console.WriteLine("Between " + period[0] + " and " + period[1] + " the total rainfall in Lund was " + totalRain + " millimeters");
			Console.ReadKey();
		}

		private static async Task ConnectAsync()
		{
			try
			{
				using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
				using (HttpResponseMessage response =
					await httpClient.GetAsync(new Uri("https://opendata.smhi.se/")))
				{
					Console.WriteLine(
						response.IsSuccessStatusCode
							? "Verification successful"
							: $"Verification failure: {response.ReasonPhrase}");
				}
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Gets XML document
		/// </summary>
		/// <param name="version"></param>
		/// <param name="parameter"></param>
		/// <param name="stationset"></param>
		/// <param name="stationId"></param>
		/// <param name="timeperiod"></param>
		/// <returns>Json data in string format</returns>

		private static XmlDocument GetData(string version = "1.0", string parameter = "1", string stationset = "station-set", string stationId = "all", string timeperiod = "latest-hour")
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("https://opendata-download-metobs.smhi.se/api/version/" + version + "/parameter/" + parameter + "/" + stationset + "/" + stationId + "/period/" + timeperiod + "/data.xml");
			return doc;
		}
		/// <summary>
		/// Gets the total rain over the last 4 months.
		/// </summary>
		/// <param name="version">Which version of the api to use</param>
		/// <param name="stationset">Specification if data is from a station or station-set</param>
		/// <param name="stationId">Which stationID to get data from</param>
		/// <param name="timeperiod">Time period to return values from</param>
		/// <returns>Average temperature</returns>
		private static async Task<float> GetTotalRain(string version = "1.0", string parameter = "1", string stationset = "station-set", string stationId = "all", string timeperiod = "latest-hour")
		{
			XmlDocument doc = GetData(version, parameter, stationset, stationId, timeperiod);
			var values = doc.GetElementsByTagName("value");
			var totalRain = 0.0f;
			foreach(XmlNode val in values)
            {
				if (val.ChildNodes.Count > 1)
					continue;
				totalRain += float.Parse(val.InnerText);
            }
			return totalRain;
		}

		/// <summary>
		/// Gets the period for a dataset.
		/// </summary>
		/// <param name="version"></param>
		/// <param name="parameter"></param>
		/// <param name="stationset"></param>
		/// <param name="stationId"></param>
		/// <param name="timeperiod"></param>
		/// <returns></returns>
		private static async Task<string[]> GetPeriod(string version = "1.0", string parameter = "1", string stationset = "station-set", string stationId = "all", string timeperiod = "latest-hour")
		{
			XmlDocument doc = GetData(version, parameter, stationset, stationId, timeperiod);
			XmlNodeList period = doc.GetElementsByTagName("period");
            string[] timeArray = new string[2];
			timeArray[0] = period[0]["from"]?.InnerText;
			timeArray[1] = period[0]["to"]?.InnerText;
			return timeArray;
		}

		public static string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}
	}
}
