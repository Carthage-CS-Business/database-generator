using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Odbc;

namespace database_generator
{
	public class City
	{
		public string city;
		public string state;
		public string zip_code;
	}

	public class MyException : System.Exception
	{
	}

	class Program
	{
		static List<string> street_names = new List<string>();
		static List<City> cities = new List<City>();
		static OdbcConnection connection = new OdbcConnection();
		static string street_table_name = "street_names";

		static void Main(string[] args)
		{
			try
			{
				OpenDatabase();
				ReadStreetNames();
				ReadCities();
				ProcessCommands();
			}
			catch (MyException)
			{
			}

			if (connection.State == System.Data.ConnectionState.Open)
				connection.Close();
			Console.Write("Enter enter to exit: ");
			Console.ReadLine();
		}

		static void OpenDatabase()
		{
			string connection_string = "Driver={MySQL ODBC 5.3 ANSI Driver};Server=192.168.1.9;Database=excel_data;User = root; Password = Ad0adid.; Option = 3; ";
			connection.ConnectionString = connection_string;
			try
			{
				connection.Open();
				Console.WriteLine("Connection to database established.");
			}
			catch (System.Data.Odbc.OdbcException e)
			{
				Console.WriteLine(e.Message);
				throw new MyException();
			}
		}

		static void ProcessCommands()
		{
			bool keep_going = true;

			while (keep_going)
			{
				char c = PrintMenu();
				switch (c)
				{
					case 's':
						ProcessStreets();
						break;

					case 'q':
						keep_going = false;
						break;
				}
			}
		}

		static int TableSize(string table_name)
		{
			int retval = 0;
			OdbcCommand cmd = new OdbcCommand("select count(*) from " + table_name);
			cmd.Connection = connection;
			try
			{
				retval = Convert.ToInt32(cmd.ExecuteScalar());
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw new MyException();
			}
			return retval;
		}
		static bool ProcessStreets()
		{
			bool retval = false;
			string line;

			Console.WriteLine("In-memory streets list length: {0}", street_names.Count());
			Console.WriteLine("Database streets list length: {0}", TableSize(street_table_name));
			Console.WriteLine("i	(re)initialize database from in-memory list");
			Console.Write("Enter choice: ");
			line = Console.ReadLine();
			if (line.Length > 0)
			{
				switch (line[0])
				{
					case 'i':
						break;
				}
			}
			return retval;
		}
		static char PrintMenu()
		{
			char retval = '\0';
			string line;
			Console.WriteLine("s	process streets");
			Console.WriteLine("q	quit");
			Console.Write("Enter choice: ");
			line = Console.ReadLine();
			if (line.Length > 0)
				retval = line[0];
			return retval;
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
		static bool ReadCities()
		{
			try
			{
				string line;
				string[] parsed_line;
				char[] separators = { ',' };
				StreamReader file = new StreamReader("CitySt.csv");
				while ((line = file.ReadLine()) != null)
				{
					parsed_line = line.Split(separators);
					if (parsed_line.Length == 5)
					{
						City c = new City();
						c.city = parsed_line[2];
						c.state = parsed_line[3];
						c.zip_code = parsed_line[4];
						cities.Add(c);
					}
				}
				file.Close();
				Console.WriteLine("{0} lines read from cites file.", cities.Count);
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine(e.Message);
			}
			return street_names.Count > 0;
		}
	}
}
