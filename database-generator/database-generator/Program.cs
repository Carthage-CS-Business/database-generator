using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace database_generator
{
	class Program
	{
		static List<string> street_names = new List<string>();

		static void Main(string[] args)
		{
			ReadStreetNames();
		}

		static bool ReadStreetNames()
		{
			try
			{
				string line;
				string[] parsed_line;
				char[] separators = { ',' };
				StreamReader file = new StreamReader("Street_Names.csv");
				while ((line = file.ReadLine()) != null)
				{
					parsed_line = line.Split(separators);
					street_names.Add(parsed_line[0]);
				}
				file.Close();
				Console.WriteLine("{0} lines read from streets file.", street_names.Count);
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine(e.Message);
			}
			return street_names.Count > 0;
		}
	}
}
