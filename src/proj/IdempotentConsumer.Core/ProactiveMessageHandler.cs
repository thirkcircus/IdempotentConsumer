namespace IdempotentConsumer.Core
{
	using NServiceBus;

	public abstract class ProactiveMessageHandler<T> : ReactiveMessageHandler<T>
		where T : IMessage
	{
		public IFilterDuplicateMessages DuplicateFilter { private get; set; }

		public override void Handle(T message)
		{
			this.Registration.RegisterMessageIdentifiers(
				this.GetAggregateId(message), this.GetMessageId(message));

			this.DuplicateFilter.Filter(
				() => this.HandleMessage(message),
				this.GetAggregateId(message),
				this.GetMessageId(message));
		}
	}
}