namespace IdempotentConsumer
{
	using NServiceBus;

	public interface IHandleMessagesOnce<T> : IHandleMessages<T>
		where T : IMessage
	{
	}
}