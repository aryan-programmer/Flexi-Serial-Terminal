namespace Flexi_Serial_Terminal {
	public enum SentCommandType {
		PollRequest,
		Command,
		ConfigurableCommand
	}

	public class SentCommandPollRequest: SentCommand {
		public PollData PollData { get; private set; }

		public SentCommandPollRequest(PollData pollData) : base(SentCommandType.PollRequest) => PollData = pollData;
	}

	public abstract class SentCommand {
		public SentCommandType SentCommandType { get; private set; }

		public SentCommand(SentCommandType sentCommandType) => SentCommandType = sentCommandType;
	}
}