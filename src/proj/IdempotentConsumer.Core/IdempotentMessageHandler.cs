namespace IdempotentConsumer.Core
{
	using System;
	using NServiceBus;

	public abstract class IdempotentMessageHandler<T> : IHandleMessagesOnce<T>
		where T : IMessage
	{
		public ICaptureDispatchedMessages Bus { get; set; }
		public IFilterDuplicateMessages DuplicateFilter { get; set; }

		public void Handle(T message)
		{
			var aggregateId = this.GetAggregateId(message);
			var messageId = this.GetMessageId(message);

			this.Bus.AssignMessageIdentifiers(aggregateId, messageId);
			this.DuplicateFilter.Filter(() => this.HandleMessage(message), aggregateId, messageId);
		}

		protected abstract void HandleMessage(T message);
		protected abstract Guid GetAggregateId(T message);
		protected abstract Guid GetMessageId(T message);
	}
}