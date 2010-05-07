namespace IdempotentConsumer
{
	using System;
	using NServiceBus;

	[Serializable]
	public class DispatchedMessage
	{
		public Guid AggregateId { get; set; }
		public Guid SourceMessageId { get; set; }

		public Guid AggregateIndex { get; set; }
		public int MessageIndex { get; set; }
		public int GroupIndex { get; set; }

		public DateTime Created { get; set; }
		public DispatchMethod Method { get; set; }

		public IMessage Body { get; set; }
	}
}