using System;
using System.Configuration;

namespace FlexiSerialTerminalSettingsClasses {
	[Serializable, SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public class PollSaveData {
		public bool   IsPolling   { get; set; }
		public string Name        { get; set; }
		public string PollCommand { get; set; }
	}

	[Serializable, SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public class PollSaveDataArray {
		public bool[]   IsPolling   { get; set; }
		public string[] Name        { get; set; }
		public string[] PollCommand { get; set; }
	}
}