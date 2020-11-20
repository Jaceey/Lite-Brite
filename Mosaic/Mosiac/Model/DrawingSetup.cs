/*!	\file		DrawingSetup.cs
	\author		Jaceey Tuck
	\date		2019-04-14
	
		DrawingSetup class declaration
			Used to store the Colour values from CSV file
*/
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosiac.Model
{
	public class DrawingSetup : INotifyPropertyChanged
	{
		string _Colour;
		[Name("Colour")]
		public string Colour
		{
			get
			{
				return _Colour;
			}
			set
			{
				if (_Colour != value)
				{
					_Colour = value;
					RaisePropertyChanged("Colour");
				}
			}
		}

		public override string ToString()
		{
			return this.Colour;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void RaisePropertyChanged(string prop)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
