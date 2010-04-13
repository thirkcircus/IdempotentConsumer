namespace IdempotentConsumer
{
	using System;
	using NServiceBus;

	public interface ICaptureDispatchedMessages : IBus
	{
		void AssignMessageIdentifiers(Guid aggregateId, Guid currentMessageId);
	}
}