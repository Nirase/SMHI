﻿using System;
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
		public static async Task Main(string[] args)
		{
			while(running)
            {
				Console.WriteLine("Please select an option:\n" +
					"2. Calculate total rainfall in Lund\n" + 
					"4. Quit");
				
				switch(Console.ReadLine())
                {
					case "2":
                        try
                        {
							Rainfall rainfall = new Rainfall();
							rainfall.Parse("52430", "1.0", "latest-months");
						}
						catch(Exception e)
                        {
							Console.WriteLine(e);
							continue;
                        }
						break;
					case "4":
						running = false;
						break;
                }
            }
		}
	}
}
