using System.IO.Ports;
using Flexi_Serial_Terminal.Properties;

namespace Flexi_Serial_Terminal {
	public class ComConnection {

		private readonly object @lock = new object();

		private SerialPort serial;

		static ComConnection() { }

		private ComConnection() { }
		public static ComConnection I { get; } = new ComConnection();

		public bool IsConnected { get; private set; }

		public event SerialDataReceivedEventHandler DataReceived;

		/// <summary>
		///     Initializes a serial connection, dropping the previous one if any,
		///     to the RS232 Serial COM port at baud rate as specified in the application settings.
		/// </summary>
		public void Connect() {
			lock (@lock) {
				if (IsConnected) return;
				serial = new SerialPort(Settings.Default.ComPort, Settings.Default.BaudRate);
				serial.Open();
				serial.DataReceived += DataReceived;
				IsConnected         =  true;
			}
		}

		/// <summary>
		///     Drops the current connection to the RS232 Serial COM port, if it exists, otherwise it does nothing.
		/// </summary>
		public void Disconnect() {
			lock (@lock) {
				if (!IsConnected)
					return;
				serial.Close();
				serial      = null;
				IsConnected = false;
			}
		}

		public void Send(string command) {
			lock (@lock) {
				serial.Write(command);
			}
		}

		public string ReadExisting() {
			lock (@lock) {
				return serial.ReadExisting();
			}
		}

		~ComConnection() {
			lock (@lock) {
				serial?.Close();
			}
		}
	}
}