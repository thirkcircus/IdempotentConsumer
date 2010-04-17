namespace IdempotentConsumer.Core
{
	using System;
	using NServiceBus;

	public abstract class IdempotentMessageHandler<T> : IHandleMessages<T>
		where T : IMessage
	{
		public IRegisterMessageIdentifiers Registration { get; set; }

		public void Handle(T message)
		{
			this.Registration.RegisterMessageIdentifiers(
				this.GetAggregateId(message), this.GetMessageId(message));
		}

		protected abstract Guid GetAggregateId(T message);
		protected abstract Guid GetMessageId(T message);
	}
}