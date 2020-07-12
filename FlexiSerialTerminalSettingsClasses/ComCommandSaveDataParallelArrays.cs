using System;
using System.Configuration;

namespace FlexiSerialTerminalSettingsClasses {
	[Serializable, SettingsSerializeAs(SettingsSerializeAs.Xml)]
	public class ComCommandSaveDataParallelArrays {
		public string[] Title    { get; set; }
		public string[] Command { get; set; }
	}
}