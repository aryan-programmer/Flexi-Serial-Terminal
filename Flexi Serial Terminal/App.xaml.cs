using System;
using System.Windows;
using Flexi_Serial_Terminal.Properties;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void App_OnExit(object sender, ExitEventArgs e) => Settings.Default.Save();
	}
}