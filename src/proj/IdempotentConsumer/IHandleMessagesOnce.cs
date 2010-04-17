namespace IdempotentConsumer
{
	using NServiceBus;

	/// <summary>
	/// Message handlers that want to have message processing be idempotent must implement this interface.
	/// Doing so will cause a special version of IBus to be injected which facilitates tracking of all
	/// work related to processing each message.
	/// </summary>
	/// <typeparam name="T">The type of message whose contents must only be processed once.</typeparam>
	public interface IHandleMessagesOnce<T> : IHandleMessages<T>
		where T : IMessage
	{
	}
}