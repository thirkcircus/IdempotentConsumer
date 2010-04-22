namespace IdempotentConsumer.Core
{
	using System;
	using NServiceBus;

	public abstract class ReactiveMessageHandler<T> : IHandleMessagesOnce<T>
		where T : IMessage
	{
		public IIdempotentBus Bus { get; set; }
		public IRegisterMessageIdentifiers Registration { get; set; }

		public virtual void Handle(T message)
		{
			this.Registration.RegisterMessageIdentifiers(
				this.GetAggregateId(message), this.GetMessageId(message));

			this.HandleMessage(message);
		}

		protected abstract void HandleMessage(T message);
		protected abstract Guid GetAggregateId(T message);
		protected abstract Guid GetMessageId(T message);
	}
}