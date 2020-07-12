using System.Windows;
using Flexi_Serial_Terminal.Properties;
using FlexiSerialTerminalSettingsClasses;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void App_OnExit(object sender, ExitEventArgs e) => Settings.Default.Save();

		private void App_OnStartup(object sender, StartupEventArgs e) {
			if (Settings.Default.PollData == null) {
				Settings.Default.PollData = new PollSaveDataParallelArrays {
					IsPolling   = new[] {false},
					Name        = new[] {"Poll name"},
					PollCommand = new[] {"POLL_CMD"}
				};
			}

			if (Settings.Default.ComCommands == null) {
				Settings.Default.ComCommands = new ComCommandSaveDataParallelArrays {
					Title   = new[] {"Cmd name"},
					Command = new[] {"SEND_CMD"}
				};
			}
		}
	}
}