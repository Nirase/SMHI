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
		static bool running = true;
		public static void Main(string[] args)
		{
			Console.ReadKey();
			ProgramLoop();
		}

		public static void ProgramLoop()
        {
			while (running)
			{
				Console.WriteLine("Please select an option:\n" +
					"1. Calculate average temperature in Sweden\n" +
					"2. Calculate total rainfall in Lund\n" +
					"3. Post the temperature at all stations\n" +
					"4. Quit");

				switch (Console.ReadLine())
				{
					case "1":
						try
						{
							Temperature temperature = new Temperature();
							Console.WriteLine("the average temperature in Sweden for the last hours was " + temperature.GetAverageTemperature() + " degrees");
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							continue;
						}
						break;
					case "2":
						try
						{
							Rainfall rainfall = new Rainfall("53430");
							var period = rainfall.GetPeriod();
							Console.WriteLine("Between " + period[0] + " and " + period[1] + " the total rainfall in " + rainfall.GetName() + " was " + rainfall.GetTotalRain() + " millimeters");
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							continue;
						}
						break;
					case "3":
						Temperature temperature2 = new Temperature();
						temperature2.Run();
						break;
					case "4":
						running = false;
						break;
				}
			}
		}
	}
}
