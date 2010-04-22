namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using IdempotentConsumer;

	public class InMemoryStorage : ILoadMessages, IStoreMessages
	{
		private readonly ICollection<DispatchedMessage> storage = new LinkedList<DispatchedMessage>();

		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			return this.storage.Where(x => x.AggregateId == aggregateId && x.SourceMessageId == messageId);
		}

		public void Persist(IEnumerable<DispatchedMessage> messages)
		{
			lock (this.storage)
			{
				this.PersistOrThrow(messages);
			}
		}
		private void PersistOrThrow(IEnumerable<DispatchedMessage> messages)
		{
			this.ThrowWhenAlreadyCommitted(messages.FirstOrDefault());
			foreach (var message in messages)
				this.storage.Add(message);
		}
		private void ThrowWhenAlreadyCommitted(DispatchedMessage first)
		{
			if (first == null)
				return;

			var committed = this.Load(first.AggregateId, first.SourceMessageId).ToArray();
			if (committed.Length != 0)
				throw new DuplicateMessageException { CommittedMessages = committed };
		}
	}
}