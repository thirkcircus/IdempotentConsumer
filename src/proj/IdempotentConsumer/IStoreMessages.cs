namespace IdempotentConsumer
{
	using System.Collections.Generic;

	public interface IStoreMessages
	{
		void Persist(IEnumerable<DispatchedMessage> messages);
	}
}