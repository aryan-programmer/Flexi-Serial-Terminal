using System;
using System.Configuration;

namespace FlexiSerialTerminalSettingsClasses {
	[Serializable, SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public class PollSaveDataParallelArrays {
		public bool[]   IsPolling   { get; set; }
		public string[] Name        { get; set; }
		public string[] PollCommand { get; set; }
	}
}