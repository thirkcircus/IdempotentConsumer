namespace IdempotentStore.LinqToSql
{
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Linq;
	using IdempotentConsumer;

	public class DataContextMessageStore : IStoreDispatchedMessages
	{
		private readonly DataContext context;

		public DataContextMessageStore(DataContext context)
		{
			this.context = context;
		}

		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			return from message in this.context.GetTable<DispatchedMessage>()
			       where message.AggregateId == aggregateId && message.SourceMessageId == messageId
			       select message;
		}
	}
}