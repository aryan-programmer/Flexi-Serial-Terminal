using System.Windows;

namespace Flexi_Serial_Terminal {
	public class BindingProxy<T> : Freezable {

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(T), typeof(BindingProxy<T>), new UIPropertyMetadata(default(T)));

		public T Data {
			get => (T) GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		protected override Freezable CreateInstanceCore() => new BindingProxy<T>();
	}
}