using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


		CancellationTokenSource cancelSource = new CancellationTokenSource();

		public void Run(string parameter = "1", string period = "latest-hour")
		{
			new Thread(async () =>
			{
				await Work(cancelSource.Token, parameter, period);
			}).Start();

			Console.WriteLine("Working");
			Console.ReadKey();
			cancelSource.Cancel();
			Console.ReadKey();
		}


		private async Task Work(CancellationToken cancelToken, string parameter = "1", string period = "latest-hour")
		{
			int i = 0;
			while (true)
			{
				DownloadRequest = new DownloadRequest();
				DownloadRequest.GetData("https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/" + parameter + "/station-set/all/period/" + period +"/data.xml");
				var stations = DownloadRequest.Document.GetElementsByTagName("station");
				await Task.Delay(100);
				if (cancelToken.IsCancellationRequested)
				{
					Console.WriteLine("Cancelling...");
					return;
				}
				Console.WriteLine(stations[i]["name"]?.InnerText + ": " + (stations[i]["value"] != null ? stations[i]["value"]["value"]?.InnerText : ""));
				++i;
			}
		}
	}
}