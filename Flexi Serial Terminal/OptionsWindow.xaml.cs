using System.IO.Ports;
using System.Windows;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : Window {
		public string[] ComPorts { get; } = SerialPort.GetPortNames();

		public uint[] BaudRates { get; } = {
			9600U,
			14400U,
			19200U,
			38400U,
			56000U,
			57600U,
			115200U,
			128000U,
			256000U
		};

		public OptionsWindow() {
			InitializeComponent();
		}
	}
}