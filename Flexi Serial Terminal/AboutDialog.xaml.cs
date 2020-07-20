using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     Interaction logic for AboutDialog.xaml
	/// </summary>
	public partial class AboutDialog : UserControl {

		public AboutDialog() {
			InitializeComponent();

			var version = "Develop";
			if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed) {
				System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
				version = ad.CurrentVersion.ToString();
			}

			NameBlock.Text = $"Flexi serial terminal {version}";
		}

		public event Action Close;

		private void Close_OnClick(object sender, RoutedEventArgs e) => Close?.Invoke();
	}
}