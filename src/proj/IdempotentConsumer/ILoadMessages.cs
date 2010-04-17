namespace IdempotentConsumer
{
	using System;
	using System.Collections.Generic;

	public interface ILoadMessages
	{
		IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId);
	}
}