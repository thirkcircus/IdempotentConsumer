namespace IdempotentConsumer
{
	using System.Collections.Generic;
	using NServiceBus;

	public interface IBuildMessages
	{
		IEnumerable<DispatchedMessage> Build(DispatchMethod method, params IMessage[] message);
	}
}