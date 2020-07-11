using System;
using System.Windows;
using System.Windows.Controls;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     Interaction logic for Command.xaml
	/// </summary>
	public partial class ComCommand : UserControl {

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public static readonly DependencyProperty CommandNameProperty =
			DependencyProperty.Register("CommandName", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public static readonly DependencyProperty CommandStatusProperty =
			DependencyProperty.Register("CommandStatus", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public ComCommand() {
			InitializeComponent();
		}


		public string Title {
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}


		public string CommandName {
			get => (string) GetValue(CommandNameProperty);
			set => SetValue(CommandNameProperty, value);
		}


		public string CommandStatus {
			get => (string) GetValue(CommandStatusProperty);
			set => SetValue(CommandStatusProperty, value);
		}

		public event Action Send;


		private void SendBtn_OnClick(object sender, RoutedEventArgs e) => Send?.Invoke();
	}
}