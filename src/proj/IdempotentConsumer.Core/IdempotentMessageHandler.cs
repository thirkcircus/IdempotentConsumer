namespace IdempotentConsumer.Core
{
	using System;
	using NServiceBus;

	public abstract class IdempotentMessageHandler<T> : IHandleMessagesOnce<T>
		where T : IMessage
	{
		public IFilterDuplicateMessages DuplicateHandler { get; set; }

		public void Handle(T message)
		{
			this.DuplicateHandler.Filter(
				() => this.HandleNewMessage(message),
				this.GetAggregateId(message),
				this.GetMessageId(message));
		}

		protected abstract void HandleNewMessage(T message);
		protected abstract Guid GetAggregateId(T message);
		protected abstract Guid GetMessageId(T message);
	}
}