namespace IdempotentConsumer
{
	using System.Collections.Generic;

	public interface IDispatchMessages
	{
		void Dispatch(IEnumerable<DispatchedMessage> messages);
	}
}