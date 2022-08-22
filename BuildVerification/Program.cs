using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuildVerification
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			ConnectAsync().Wait();

			Console.WriteLine();
			Console.WriteLine("Press any key to continue...");
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
	}
}
