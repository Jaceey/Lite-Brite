/*!	\file		MainWindow.xaml.cs
	\author		Jaceey Tuck
	\date		2019-04-14
	
		Code behind of the Mosaic board
*/
using Mosiac.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mosiac
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ViewModelMain vm;
		private Point startPoint;
		public MainWindow()
		{
			InitializeComponent();

			vm = new ViewModelMain(mosiacBoard);
			this.DataContext = vm;
		}

		private void Ellipse_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			startPoint = e.GetPosition(null);   // absolute position
		}

		private void Ellipse_MouseMove(object sender, MouseEventArgs e)
		{
			Ellipse ellipse = sender as Ellipse;
			string theItem = ellipse.Fill.ToString();

			DataObject dragData = new DataObject(typeof(string), theItem);
			DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
		}      

	}
}
