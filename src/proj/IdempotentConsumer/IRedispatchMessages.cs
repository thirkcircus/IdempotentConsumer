namespace IdempotentConsumer
{
	using System.Collections.Generic;

	public interface IRedispatchMessages
	{
		void Redispatch(IEnumerable<DispatchedMessage> messages);
	}
}