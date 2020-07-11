using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Flexi_Serial_Terminal {

	public class PollData : DependencyObject, IDisposable {

		public PollData() => ChooseSaveFileCommand = new RelayCommand(_ => ChooseSaveFile());

		public bool IsPolling {
			get => (bool) GetValue(IsPollingProperty);
			set => SetValue(IsPollingProperty, value);
		}

		public static readonly DependencyProperty IsPollingProperty =
			DependencyProperty.Register("IsPolling", typeof(bool), typeof(PollData), new PropertyMetadata(false));

		public string PollCommand {
			get => (string) GetValue(PollCommandProperty);
			set => SetValue(PollCommandProperty, value);
		}

		public static readonly DependencyProperty PollCommandProperty =
			DependencyProperty.Register("PollCommand", typeof(string), typeof(PollData), new PropertyMetadata(""));

		public string Name {
			get => (string) GetValue(NameProperty);
			set => SetValue(NameProperty, value);
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(PollData), new PropertyMetadata(""));

		public string Value {
			get => (string) GetValue(ValueProperty);
			private set => SetValue(ValueProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(string), typeof(PollData), new PropertyMetadata(""));

		public string SaveFilePath {
			get => (string) GetValue(SaveFilePathProperty);
			set => SetValue(SaveFilePathProperty, value);
		}

		public static readonly DependencyProperty SaveFilePathProperty =
			DependencyProperty.Register("SaveFilePath", typeof(string), typeof(PollData), new PropertyMetadata(""));

		private StreamWriter fileStream;

		private readonly LinkedList<string> pastValues = new LinkedList<string>();

		public ICommand ChooseSaveFileCommand { get; }

		public void SaveValue(string value) {
			Value = value;
			if (fileStream == null) {
				pastValues.AddLast(Value);
			} else {
				fileStream.Write(Value + ",");
				fileStream.Flush();
			}
		}

		private void ChooseSaveFile() {
			var file = new SaveFileDialog {
				Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
			};
			file.ShowDialog();
			if (string.IsNullOrEmpty(file.FileName)) return;
			SaveFilePath = file.FileName;
			fileStream   = new StreamWriter(path: file.FileName, append: true);
			fileStream.Write(pastValues.Aggregate("", (res, pastValue) => res + pastValue + ","));
			fileStream.Flush();
		}

		#region IDisposable

		public void Dispose() {
			fileStream?.Dispose();
			fileStream = null;
		}

		#endregion

	}
}