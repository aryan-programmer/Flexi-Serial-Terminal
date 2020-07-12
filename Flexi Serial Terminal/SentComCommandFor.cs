namespace Flexi_Serial_Terminal {

	public abstract class SentComCommandFor {
		public class PollRequest : SentComCommandFor {
			public PollRequest(PollData pollData) => PollData = pollData;
			public PollData PollData { get; }
		}

		public class SendCommand : SentComCommandFor {
			public SendCommand(ComCommand comCommand) => ComCommand = comCommand;
			public ComCommand ComCommand { get; }
		}
	}
}