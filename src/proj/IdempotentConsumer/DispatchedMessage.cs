namespace IdempotentConsumer
{
	using System;
	using NServiceBus;

	public class DispatchedMessage
	{
		public Guid AggregateId { get; set; }
		public Guid SourceMessageId { get; set; }
		//// public string CorrelationId { get; set; } // TODO?
		//// public string MessageHeaders { get; set; } // TODO?
		public DispatchMethod Method { get; set; }
		public int GroupIndex { get; set; }
		public int MessageIndex { get; set; }
		public IMessage Payload { get; set; }
	}
}