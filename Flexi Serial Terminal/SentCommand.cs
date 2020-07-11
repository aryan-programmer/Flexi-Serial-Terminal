namespace Flexi_Serial_Terminal {
	public enum SentCommandType {
		PollRequest,
		Command,
		ConfigurableCommand
	}

	public class SentCommandPollRequest : SentCommand {

		public SentCommandPollRequest(PollData pollData) : base(SentCommandType.PollRequest) => PollData = pollData;
		public PollData PollData { get; }
	}

	public abstract class SentCommand {

		public SentCommand(SentCommandType sentCommandType) => SentCommandType = sentCommandType;
		public SentCommandType SentCommandType { get; }
	}
}