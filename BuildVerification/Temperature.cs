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
