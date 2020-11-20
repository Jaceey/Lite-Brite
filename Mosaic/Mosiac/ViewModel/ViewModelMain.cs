/*!	\file		ViewModelMain.cs
	\author		Jaceey Tuck
	\date		2019-04-14
	
		ViewModelMain class declaration            
*/
using CsvHelper;
using Mosiac.Helpers;
using Mosiac.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mosiac.ViewModel
{
	class ViewModelMain : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public Grid board = new Grid();

		public FileSetup file;

		Random random = new Random();
		public List<string> colours = new List<string>() {"Red", "Fuchsia", "HotPink", "Orange", "OrangeRed", "White", "Blue", "DodgerBlue", "LightSkyBlue", "Purple", "Violet", "Green", "GreenYellow", "Yellow", "Gray", "Black" };

		public ViewModelMain(Grid mosiacBoard)
		{
			board = mosiacBoard;

			OpenFileClick = new RelayCommand(LoadFile);
			SaveFileClick = new RelayCommand(SaveFile);
			CloseClick = new RelayCommand(CloseApp);
			AboutClick = new RelayCommand(DisplayAbout);
			NewGameClick = new RelayCommand(StartGame);
			RandomClick = new RelayCommand(DrawRandom);
		}

		private void DrawRandom(object obj)
		{   // Fills the board with randomly selected tiles/pegs
			if (board.Children.Count > 0)
			{
				board.Children.Clear();
			}
			else
			{
				// Clears current items from board and resets the program
				for (int i = 0; i < 50; i++)
				{
					board.ColumnDefinitions.Add(new ColumnDefinition());
					board.RowDefinitions.Add(new RowDefinition());
				}
			}


			for (int i = 0; i < 50; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					Ellipse el = new Ellipse();
					el.Drop += ElDestination_Drop;
					el.DragEnter += El_DragEnter;
					el.AllowDrop = true;
					Grid.SetColumn(el, j);
					Grid.SetRow(el, i);
					el.Fill = new BrushConverter().ConvertFromString(colours[RandomNumber()]) as SolidColorBrush;
					el.Width = 12;
					el.Height = 12;
					el.Margin = new Thickness(0);
					el.StrokeThickness = 0;
					board.Children.Add(el);
				}
			}
		}

		public int RandomNumber()
		{
			return random.Next(0, colours.Count);
		}

		private void StartGame(object obj)
		{
			if (board.Children.Count > 0)
			{
				board.Children.Clear();
			}
			else
			{
				// Clears current items from board and resets the program
				for (int i = 0; i < 50; i++)
				{
					board.ColumnDefinitions.Add(new ColumnDefinition());
					board.RowDefinitions.Add(new RowDefinition());
				}
			}


			for (int i = 0; i < 50; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					Ellipse el = new Ellipse();
					el.Drop += ElDestination_Drop;
					el.DragEnter += El_DragEnter;
					el.AllowDrop = true;
					Grid.SetColumn(el, j);
					Grid.SetRow(el, i);
					el.Fill = Brushes.Gray;
					el.Width = 12;
					el.Height = 12;
					el.Margin = new Thickness(0);
					el.StrokeThickness = 0;
					board.Children.Add(el);
				}
			}
		}

		private void El_DragEnter(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(typeof(string)) || sender == e.Source)
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void ElDestination_Drop(object sender, DragEventArgs e)
		{
			// If the data is a string 
			if (e.Data.GetDataPresent(typeof(string)))
			{
				// get the string from the DataObject and add to destination ListBox
				String theItem = e.Data.GetData(typeof(string)).ToString();

				Ellipse ellipse = sender as Ellipse;
				ellipse.Fill = new BrushConverter().ConvertFromString(theItem) as SolidColorBrush;

			}
		}

		private void DisplayAbout(object obj)
		{
			// Open a message box with your name in it
			MessageBox.Show(Properties.Resources.CreatedBy, "Mosiac Board About", MessageBoxButton.OK);
		}

		private void CloseApp(object obj)
		{   // Exits the program
			Application.Current.MainWindow.Close();
		}

		private void SaveFile(object obj)
		{  
			// Get current directory
			string currentDir = Directory.GetCurrentDirectory();
			// Get parent director
			string pa = Directory.GetParent(currentDir).Parent.FullName;
			// Set path to Data folder
			var path = System.IO.Path.Combine(pa, "Mosiac Art");

			// Create SaveFileDialog
			Microsoft.Win32.SaveFileDialog saveFileDlg = new Microsoft.Win32.SaveFileDialog
			{
				Filter = "Data Collection Files(*.csv;)| *.csv; |All files(*.*)| *.*",
				InitialDirectory = path
			};

			if (saveFileDlg.ShowDialog() == true)
			{
				var filePath = saveFileDlg.FileName;
				var writer = new StreamWriter(filePath);
				var csv = new CsvWriter(writer);

				csv.WriteField("Colour");
				csv.NextRecord();

				foreach (var item in board.Children)
				{
					Ellipse el = item as Ellipse;
					csv.WriteField(el.Fill);
					csv.NextRecord();
				}
				writer.Flush();
				writer.Close();
			}


		}

		// Opens a data file and auto loads a saved Mosiac file
		private void LoadFile(object obj)
		{   
			// Get current directory
			string currentDir = Directory.GetCurrentDirectory();
			// Get parent director
			string pa = Directory.GetParent(currentDir).Parent.FullName;
			// Set path to Data folder
			var path = System.IO.Path.Combine(pa, "Mosiac Art");

			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "Data Collection Files(*.csv;)| *.csv;|All files(*.*)| *.*",
				InitialDirectory = path

			};

			if (openFileDlg.ShowDialog() == true)
			{
				try
				{
					//Get the path of specified file
					var filePath = openFileDlg.FileName;
					string ext = System.IO.Path.GetExtension(openFileDlg.FileName);

					file = new FileSetup(filePath, ext);
					LoadData();

				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Looks like something broke!");
				}

			}
		}

		private void LoadData()
		{
			if (board.Children.Count > 0)
			{
				board.Children.Clear();
			}
			else
			{
				// Clears current items from board and resets the program
				for (int i = 0; i < 50; i++)
				{
					board.ColumnDefinitions.Add(new ColumnDefinition());
					board.RowDefinitions.Add(new RowDefinition());
				}
			}

			int count = 0;

			for (int i = 0; i < 50; i++)
			{
				for (int j = 0; j < 50; j++)
				{
					Ellipse el = new Ellipse();
					el.Drop += ElDestination_Drop;
					el.DragEnter += El_DragEnter;
					el.AllowDrop = true;
					Grid.SetColumn(el, j);
					Grid.SetRow(el, i);
					string test = file.drawingList[count].ToString();

					el.Fill = new BrushConverter().ConvertFromString(file.drawingList[count].ToString()) as SolidColorBrush; 
					el.Width = 12;
					el.Height = 12;
					el.Margin = new Thickness(0);
					el.StrokeThickness = 0;
					board.Children.Add(el);
					count++;
				}
			}
		}

		public RelayCommand OpenFileClick { get; set; }
		public RelayCommand SaveFileClick { get; set; }
		public RelayCommand CloseClick { get; set; }
		public RelayCommand AboutClick { get; set; }
		public RelayCommand NewGameClick { get; set; }
		public RelayCommand RandomClick { get; set; }
		internal void RaisePropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
