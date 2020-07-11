using System.Windows.Input;

namespace Flexi_Serial_Terminal {
	public static class CustomCommands {
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
			 "Exit",
			 "Exit",
			 typeof(CustomCommands),
			 new InputGestureCollection {
				 new KeyGesture(Key.F4, ModifierKeys.Alt)
			 }
			);

		public static readonly RoutedUICommand COMConnOptions = new RoutedUICommand
			(
			 "COM Connection Settings",
			 "COM Connection Settings",
			 typeof(CustomCommands),
			 new InputGestureCollection());

		public static readonly RoutedUICommand Connect = new RoutedUICommand
			(
			 "Connect COM Port",
			 "Connect COM Port",
			 typeof(CustomCommands),
			 new InputGestureCollection());

		public static readonly RoutedUICommand Disconnect = new RoutedUICommand
			(
			 "Disconnect COM Port",
			 "Disconnect COM Port",
			 typeof(CustomCommands),
			 new InputGestureCollection());
	}
}
