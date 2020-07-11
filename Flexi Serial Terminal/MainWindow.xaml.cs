using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Flexi_Serial_Terminal.Annotations;
using Flexi_Serial_Terminal.Properties;
using FlexiSerialTerminalSettingsClasses;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public sealed partial class MainWindow : MaterialWindow, INotifyPropertyChanged {
		public const string DialogHostId = "DialogHost";

		private readonly object sentCommandLock = new object();

		private ushort individualPollInterval;

		private BackgroundWorker pollingThread;

		private SentCommand sentCommand;

		public MainWindow() {
			if (Settings.Default.PollData != null) {
				PollSaveDataArray pollData = Settings.Default.PollData;
				for (var i = 0; i < pollData.Name.Length; i++)
					PollDataCollection.Add(new PollData {
						PollCommand = pollData.PollCommand[i],
						IsPolling   = pollData.IsPolling[i],
						Name        = pollData.Name[i]
					});
			}

			individualPollInterval = IndividualPollInterval = Settings.Default.IndividualPollInterval;
			DependencyPropertyDescriptor.FromProperty(IndividualPollIntervalProperty, typeof(MainWindow))
										.AddValueChanged(this,
														 (_, a) =>
															 individualPollInterval = IndividualPollInterval);

			InitializeComponent();

			DependencyPropertyDescriptor isPollingPropdpDesc =
				DependencyPropertyDescriptor.FromProperty(PollData.IsPollingProperty, typeof(PollData));
			void PollDataIsPollingChanged(object _, EventArgs __) => OnPropertyChanged(nameof(AreAllPolling));

			foreach (PollData pollData in PollDataCollection)
				isPollingPropdpDesc.AddValueChanged(pollData, PollDataIsPollingChanged);

			PollDataCollection.CollectionChanged += (sender, args) => {
				if ((args.Action == NotifyCollectionChangedAction.Remove)  ||
					(args.Action == NotifyCollectionChangedAction.Replace) ||
					(args.Action == NotifyCollectionChangedAction.Reset))
					foreach (PollData pollData in args.OldItems) {
						isPollingPropdpDesc.RemoveValueChanged(pollData, PollDataIsPollingChanged);
						pollData.Dispose();
					}

				if (args.NewItems != null)
					foreach (PollData pollData in args.NewItems)
						isPollingPropdpDesc.AddValueChanged(pollData, PollDataIsPollingChanged);

				OnPropertyChanged(nameof(AreAllPolling));
			};

			ComConnection.I.DataReceived += OnComDataReceived;
		}

		public ObservableCollection<PollData> PollDataCollection { get; set; } = new ObservableCollection<PollData>();

		public bool? AreAllPolling {
			get {
				List<bool> selected = PollDataCollection.Select(item => item.IsPolling).Distinct().ToList();
				return selected.Count == 1 ? selected[0] : (bool?) null;
			}
			set {
				if (!value.HasValue) return;
				var val                                                              = value.Value;
				foreach (PollData pollData in PollDataCollection) pollData.IsPolling = val;

				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private TResult G<TResult>(Func<TResult> callback) => Dispatcher.Invoke(callback);

		private void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

		private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			Close();
			Environment.Exit(0);
		}

		private async void COMCommOptions_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			var optionsDialog   = new OptionsDialog();
			var settingsUpdated = false;
			optionsDialog.Close += v => {
				DialogHost.CloseDialogCommand.Execute(null, DialogHost);
				settingsUpdated = v;
			};
			await DialogHost.ShowDialog(optionsDialog);
			if (!settingsUpdated) return;
			try {
				ComConnection.I.Disconnect();
				ComConnection.I.Connect();
			} catch (Exception ex) {
				Console.Write(ex);
				await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
					Title         = "Error!",
					Message       = $"Failed to reconnect to serial port with the new settings\n{ex.Message}",
					OkButtonLabel = "Ok"
				});
				return;
			}

			await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
				Title         = "Success!",
				Message       = "Successfully connected to the serial port with the new settings",
				OkButtonLabel = "Ok"
			});
		}

		private void Connect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = !ComConnection.I.IsConnected;

		private void Disconnect_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = ComConnection.I.IsConnected;

		private async void Connect_OnExecuted(object sender, ExecutedRoutedEventArgs e) => await Connect();

		private async Task Connect() {
			try {
				ComConnection.I.Connect();
			} catch (Exception ex) {
				Console.Write(ex);
				await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
					Title         = "Error!",
					Message       = $"Failed to connect to serial port\n{ex.Message}",
					OkButtonLabel = "Ok"
				});
				return;
			}

			await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
				Title         = "Success!",
				Message       = "Successfully connected to the serial port",
				OkButtonLabel = "Ok"
			});
		}

		private async void Disconnect_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			try {
				ComConnection.I.Disconnect();
			} catch (Exception ex) {
				Console.Write(ex);
				await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
					Title         = "Error!",
					Message       = $"Error: failed to disconnect from the serial port\n{ex.Message}",
					OkButtonLabel = "Ok"
				});
				return;
			}

			await AlertDialog.ShowDialogAsync(DialogHost, new AlertDialogArguments {
				Title         = "Success!",
				Message       = "Successfully disconnect from the serial port",
				OkButtonLabel = "Ok"
			});
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
			pollingThread?.CancelAsync();
			Settings.Default.PollData = new PollSaveDataArray {
				PollCommand = PollDataCollection.Select(data => data.PollCommand).ToArray(),
				IsPolling   = PollDataCollection.Select(data => data.IsPolling).ToArray(),
				Name        = PollDataCollection.Select(data => data.Name).ToArray()
			};
			Settings.Default.IndividualPollInterval = IndividualPollInterval;
			foreach (PollData pollData in PollDataCollection) pollData.Dispose();
		}

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private void AddPollDataRow_OnClick(object sender, RoutedEventArgs e) => PollDataCollection.Add(new PollData());

		private async void TogglePollingBtn_OnClick(object sender, RoutedEventArgs e) {
			if (!ComConnection.I.IsConnected) await Connect();

			if (pollingThread == null) {
				var pollingBw = new BackgroundWorker {
					WorkerSupportsCancellation = true
				};
				pollingBw.DoWork += (_, args) => SendPollRequests(pollingBw, args);
				pollingBw.RunWorkerAsync();
				pollingThread            = pollingBw;
				TogglePollingBtn.Content = "Stop polling";
			} else {
				pollingThread.CancelAsync();
				pollingThread            = null;
				TogglePollingBtn.Content = "Start polling";
			}
		}

		private void SendPollRequests(BackgroundWorker bw, DoWorkEventArgs e) {
			var currentPollDataIndex = -1;
			while (true) {
				if (bw.CancellationPending) {
					e.Cancel = true;
					return;
				}

				Thread.Sleep(individualPollInterval);
				{
					var finishedOneRound = false;

					do {
						++currentPollDataIndex;

						if (currentPollDataIndex < PollDataCollection.Count) continue;
						currentPollDataIndex = 0;
						if (finishedOneRound) goto continueOuterWhile;

						finishedOneRound = true;
						// ReSharper disable once AccessToModifiedClosure
					} while (!G(() => PollDataCollection[currentPollDataIndex].IsPolling));
				}

				lock (sentCommandLock) {
					PollData pollData = PollDataCollection[currentPollDataIndex];
					ComConnection.I.Send(G(() => pollData.PollCommand));
					sentCommand = new SentCommandPollRequest(pollData);
				}

				continueOuterWhile: ;
			}
		}

		private void OnComDataReceived(object sender, SerialDataReceivedEventArgs args) {
			lock (sentCommandLock) {
				var got = ComConnection.I.ReadExisting();
				if (sentCommand == null) return;
				switch (sentCommand) {
				case SentCommandPollRequest pollRequest:
					Dispatcher.Invoke(() => pollRequest.PollData.SaveValue(got));
					break;
				}
			}
		}

		private void AddComCommandBtn_Click(object sender, RoutedEventArgs e) {
			ComCommandPanel.Children.Add(new ComCommand {
				Margin = new Thickness(5)
			});
		}

		#region Dependency Property: ushort IndividualPollInterval

		public ushort IndividualPollInterval {
			get => (ushort) GetValue(IndividualPollIntervalProperty);
			set => SetValue(IndividualPollIntervalProperty, value);
		}

		// Using a DependencyProperty as the backing store for IndividualPollInterval.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IndividualPollIntervalProperty =
			DependencyProperty.Register("IndividualPollInterval", typeof(ushort), typeof(MainWindow),
										new PropertyMetadata((ushort) 0));

		#endregion

	}
}