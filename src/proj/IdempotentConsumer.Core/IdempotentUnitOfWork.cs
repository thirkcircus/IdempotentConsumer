namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;
	using NServiceBus;

	public class IdempotentUnitOfWork : ITrackUnitOfWork
	{
		private readonly ICollection<DispatchedMessage> tracked = new LinkedList<DispatchedMessage>();
		private readonly IStoreMessages storage;
		private readonly IDispatchMessages dispatcher;
		private readonly IBuildMessages builder;

		public IdempotentUnitOfWork(IStoreMessages storage, IDispatchMessages dispatcher, IBuildMessages builder)
		{
			this.storage = storage;
			this.dispatcher = dispatcher;
			this.builder = builder;
		}

		public void RegisterNew(DispatchMethod method, params IMessage[] messages)
		{
			foreach (var message in this.builder.Build(method, messages))
				this.tracked.Add(message);
		}
		public void Complete()
		{
			if (this.tracked.Count == 0)
				return;

			try
			{
				this.storage.Persist(this.tracked);
			}
			catch (DuplicateMessageException e)
			{
				this.tracked.Clear();

				foreach (var message in e.CommittedMessages)
					this.tracked.Add(message);
			}

			this.dispatcher.Dispatch(this.tracked);
			this.tracked.Clear();
		}
	}
}