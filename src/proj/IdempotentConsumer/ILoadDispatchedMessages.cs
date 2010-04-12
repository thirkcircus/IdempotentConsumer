namespace IdempotentConsumer
{
	using System;
	using System.Collections.Generic;

	public interface ILoadDispatchedMessages
	{
		IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId);
	}
}