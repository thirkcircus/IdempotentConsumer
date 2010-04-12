namespace IdempotentStore.LinqToSql
{
	using System;
	using System.Collections.Generic;
	using System.Data.Linq;
	using System.Linq;
	using IdempotentConsumer;

	public class DataContextMessageStore : ILoadDispatchedMessages
	{
		private readonly DataContext context;

		public DataContextMessageStore(DataContext context)
		{
			this.context = context;
		}

		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			var table = this.context.GetTable<DispatchedMessage>();
			return from message in table
			       where message.AggregateId == aggregateId && message.SourceMessageId == messageId
			       select message;
		}
	}
}