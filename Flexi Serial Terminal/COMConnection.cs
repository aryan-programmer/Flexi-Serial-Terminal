using System.IO.Ports;
using Flexi_Serial_Terminal.Properties;

namespace Flexi_Serial_Terminal {
	public static class ComConnection {
		private static SerialPort serial;

		public static bool IsConnected { get; private set; }

		/// <summary>
		/// Initializes a serial connection, dropping the previous one if any,
		/// to the RS232 Serial COM port at baud rate as specified in the application settings.
		/// </summary>
		public static void Connect() {
			if (IsConnected) Disconnect();
			serial = new SerialPort(Settings.Default.ComPort, Settings.Default.BaudRate);
			serial.Open();
			IsConnected = true;
		}

		/// <summary>
		/// Drops the current connection to the RS232 Serial COM port, if it exists, otherwise it does nothing.
		/// </summary>
		public static void Disconnect() {
			if (!IsConnected) return;
			serial.Close();
			serial      = null;
			IsConnected = false;
		}
	}
}