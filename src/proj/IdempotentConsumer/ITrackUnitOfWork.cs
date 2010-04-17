namespace IdempotentConsumer
{
	using NServiceBus;

	public interface ITrackUnitOfWork
	{
		void RegisterNew(DispatchMethod method, params IMessage[] messages);
		void Complete();
	}
}