/*!	\file		FileSetup.cs
	\author		Jaceey Tuck
	\date		2019-04-14
	
		FileSetup class declaration
			Used to read in CSV file to the DrawingSetup list
*/
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosiac.Model
{
	public class FileSetup
	{
		public List<DrawingSetup> drawingList;

		public string FileName;
		public string ShortFileName;
		public string FileType;

		public FileSetup(string filename, string filetype)
		{
			FileName = filename;
			FileType = filetype;

			LoadFile();
		}

		void LoadFile()
		{
			try
			{
				drawingList = new List<DrawingSetup>();
				using (TextReader reader = File.OpenText(FileName))
				{
					CsvReader csv = new CsvReader(reader);
					csv.Configuration.Delimiter = ",";
					csv.Configuration.MissingFieldFound = null;                   

					while (csv.Read())
					{
						DrawingSetup drawing = csv.GetRecord<DrawingSetup>();
						drawingList.Add(drawing);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
