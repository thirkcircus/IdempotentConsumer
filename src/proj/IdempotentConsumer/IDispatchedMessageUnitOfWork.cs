namespace IdempotentConsumer
{
	public interface IDispatchedMessageUnitOfWork
	{
		void RegisterNew(DispatchedMessage message);
		void Complete();
	}
}