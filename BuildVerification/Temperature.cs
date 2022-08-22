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
		Dataset Dataset { get; set; }

		public Temperature()

        {
			Dataset = new Dataset();
        }
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

			try
            {
				Dataset.GetData(url, true);

			}
			catch(Exception e)
            {
				throw e;
            }

			var temperature = 0.0f;
			try
			{
				foreach (var value in Dataset.Data)
				{
					temperature += float.Parse(value.Value);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw e;
			}

			return temperature / Dataset.Data.Count;
		}


		CancellationTokenSource cancelSource = new CancellationTokenSource();

		public void Run(string parameter = "1", string period = "latest-hour")
		{
			new Thread(async () =>
			{
				await Work(cancelSource.Token, parameter, period);
			}).Start();

			Console.ReadKey();
			cancelSource.Cancel();
		}


		private async Task Work(CancellationToken cancelToken, string parameter = "1", string period = "latest-hour", int delay = 100)
		{
			int i = 0;
			try
            {
				Dataset.GetData("https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/" + parameter + "/station-set/all/period/" + period + "/data.xml", true);

			}
			catch(Exception e)
            {
				Console.WriteLine(e);
				return;
            }
			var stations = Dataset.Document.GetElementsByTagName("station");

			while (true)
			{
				await Task.Delay(delay);
				if (cancelToken.IsCancellationRequested)
				{
					Console.WriteLine("Cancelling...");
					return;
				}
				Console.WriteLine(Dataset.Data[i].StationName + ": " + Dataset.Data[i].Value);
				++i;
			}
		}
	}
}