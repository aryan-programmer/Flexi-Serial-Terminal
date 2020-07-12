using System;
using System.Windows;
using System.Windows.Controls;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for AboutDialog.xaml
	/// </summary>
	public partial class AboutDialog: UserControl {
		public event Action Close;

		public AboutDialog() => InitializeComponent();

		private void Close_OnClick(object sender, RoutedEventArgs e) => Close?.Invoke();
	}
}
