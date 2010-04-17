namespace IdempotentStore.NHibernate
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::NHibernate;
	using global::NHibernate.Linq;
	using IdempotentConsumer;

	public class NHibernateMessageStore : ILoadMessages, IStoreMessages
	{
		private readonly ISession session;

		public NHibernateMessageStore(ISession session)
		{
			this.session = session;
		}

		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			return from message in this.session.Linq<DispatchedMessage>()
			       where message.AggregateId == aggregateId && message.SourceMessageId == messageId
			       select message;
		}

		public void Persist(IEnumerable<DispatchedMessage> messages)
		{
			foreach (var message in messages)
				this.session.Save(message);
		}
	}
}