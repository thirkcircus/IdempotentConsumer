namespace IdempotentConsumer
{
	using System.Collections.Generic;

	public interface IStoreDispatchedMessages
	{
		void Store(ICollection<DispatchedMessage> messages);
	}
}