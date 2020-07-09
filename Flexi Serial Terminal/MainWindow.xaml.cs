using System;
using System.Windows;
using System.Windows.Input;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MaterialWindow {
		public MainWindow() {
			InitializeComponent();
		}

		private void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

		private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			Close();
			Environment.Exit(0);
		}

		private async void COMCommOptions_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			//var optDialog = new OptionsWindow();
			//optDialog.ShowDialog();
			var optDiag = new OptionsDialog();
			var ret     = false;
			optDiag.Close += b => {
				DialogHost.CloseDialogCommand.Execute(null, DialogHost);
				ret = b;
			};
			await DialogHost.ShowDialog(optDiag);
			Console.WriteLine(ret);
		}

		private void Connect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = !ComConnection.IsConnected;

		private void Disconnect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = ComConnection.IsConnected;

		private async void Connect_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			try {
				ComConnection.Connect();
			} catch (Exception ex) {
				Console.Write(ex);
				await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments() {
					Title         = "Error: failed to connect to serial port",
					Message       = ex.Message,
					OkButtonLabel = "Ok"
				});
				return;
			}

			await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments() {
				Title         = "Successfully connected to the serial port",
				OkButtonLabel = "Ok"
			});
		}

		private async void Disconnect_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			try {
				ComConnection.Disconnect();
			} catch (Exception ex) {
				Console.Write(ex);
				await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments() {
					Title         = "Error: failed to disconnect from the serial port",
					Message       = ex.Message,
					OkButtonLabel = "Ok"
				});
				return;
			}

			await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments() {
				Title         = "Successfully disconnect from the serial port",
				OkButtonLabel = "Ok"
			});
		}

		private void SaveFileControl_FileSelected(object sender, RoutedEventArgs e) { }

		private void SaveFileControl_Cancel(object sender, RoutedEventArgs e) { }
	}
}