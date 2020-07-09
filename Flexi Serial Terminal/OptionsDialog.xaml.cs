using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using Flexi_Serial_Terminal.Properties;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsDialog : UserControl {
		public event Action<bool> Close;

		public string[] ComPorts { get; } = SerialPort.GetPortNames();

		public static int[] BaudRates { get; } = {
			9600,
			14400,
			19200,
			38400,
			56000,
			57600,
			115200,
			128000,
			256000
		};

		public OptionsDialog() {
			InitializeComponent();

			ComPortBox.SelectedIndex  = Array.IndexOf(ComPorts,  Settings.Default.ComPort);
			BaudRateBox.SelectedIndex = Array.IndexOf(BaudRates, Settings.Default.BaudRate);
		}

		private void Save_OnClick(object sender, RoutedEventArgs e) {
			var settingsSaved = false;

			if ((ComPortBox.SelectedValue          != null) &&
				((string) ComPortBox.SelectedValue != Settings.Default.ComPort)) {
				Settings.Default.ComPort = (string) ComPortBox.SelectedValue;
				settingsSaved            = true;
			}

			if ((BaudRateBox.SelectedValue        != null) &&
				((int) BaudRateBox.SelectedValue != Settings.Default.BaudRate)) {
				Settings.Default.BaudRate = BaudRates[BaudRateBox.SelectedIndex];
				settingsSaved             = true;
			}
			Close?.Invoke(settingsSaved);
		}

		private void Cancel_OnClick(object sender, RoutedEventArgs e) => Close?.Invoke(false);
	}
}