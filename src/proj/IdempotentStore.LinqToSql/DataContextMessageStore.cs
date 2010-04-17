namespace IdempotentStore.LinqToSql
{
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Linq;
	using IdempotentConsumer;

	public class DataContextMessageStore : ILoadMessages, IStoreMessages
	{
		private readonly Table<DispatchedMessage> table;

		public DataContextMessageStore(DataContext context)
		{
			this.table = context.GetTable<DispatchedMessage>();
		}

		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			return from message in this.table
			       where message.AggregateId == aggregateId && message.SourceMessageId == messageId
			       select message;
		}

		public void Persist(IEnumerable<DispatchedMessage> messages)
		{
			this.table.AttachAll(messages);
		}
	}
}