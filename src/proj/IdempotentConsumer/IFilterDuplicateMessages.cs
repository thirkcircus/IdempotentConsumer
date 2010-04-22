namespace IdempotentConsumer
{
	using System;

	public interface IFilterDuplicateMessages
	{
		void Filter(Action handleMessage, Guid aggregateId, Guid messageId);
	}
}