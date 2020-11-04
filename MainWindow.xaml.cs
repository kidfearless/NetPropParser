using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace NetPropParser
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ParseButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();

			if((bool)dialog.ShowDialog())
			{
				var stream = dialog.OpenFile();
				using var reader = new StreamReader(stream);
				var lines = reader.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
				var parsed = Parser.Parse(lines);
				var serial = Parser.Serialize(parsed);
				File.WriteAllText(dialog.FileName + ".cs", serial);
			}
		}
	}
}
