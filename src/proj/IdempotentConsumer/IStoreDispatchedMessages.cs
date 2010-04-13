namespace IdempotentConsumer
{
	using System;
	using System.Collections.Generic;

	public interface IStoreDispatchedMessages
	{
		IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId);
	}
}