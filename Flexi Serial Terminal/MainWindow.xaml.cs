using System;
using System.Windows;
using System.Windows.Input;

namespace Flexi_Serial_Terminal {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private bool isConnected;

		public MainWindow() {
			InitializeComponent();
		}

		private void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

		private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			Close();
			Environment.Exit(0);
		}

		private void COMCommOptions_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			var optDialog = new OptionsWindow();
			optDialog.ShowDialog();
		}

		private void Connect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = !isConnected;
		private void Disconnect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = isConnected;

		private void Connect_OnExecuted(object sender, ExecutedRoutedEventArgs e) => isConnected = true;
		private void Disconnect_OnExecuted(object sender, ExecutedRoutedEventArgs e) => isConnected = false;
	}
}