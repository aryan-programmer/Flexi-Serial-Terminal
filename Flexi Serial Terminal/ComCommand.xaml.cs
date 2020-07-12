using System.Windows;
using System.Windows.Controls;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     Interaction logic for Command.xaml
	/// </summary>
	public partial class ComCommand : UserControl {

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public static readonly DependencyProperty CommandStatusProperty =
			DependencyProperty.Register("CommandStatus", typeof(string), typeof(ComCommand), new PropertyMetadata(""));

		public static readonly RoutedEvent SendEvent =
			EventManager.RegisterRoutedEvent("Send", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
											 typeof(ComCommand));

		public static readonly RoutedEvent CloseEvent =
			EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
											 typeof(ComCommand));

		public ComCommand() {
			InitializeComponent();
		}

		public string Title {
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}


		public string Command {
			get => (string) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}


		public string CommandStatus {
			get => (string) GetValue(CommandStatusProperty);
			set => SetValue(CommandStatusProperty, value);
		}

		public event RoutedEventHandler Send {
			add => AddHandler(SendEvent, value);
			remove => RemoveHandler(SendEvent, value);
		}

		public event RoutedEventHandler Close {
			add => AddHandler(CloseEvent, value);
			remove => RemoveHandler(CloseEvent, value);
		}

		private void SendBtn_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(SendEvent));

		private void Close_OnClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CloseEvent));
	}
}