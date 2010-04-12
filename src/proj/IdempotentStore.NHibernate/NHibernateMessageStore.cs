namespace IdempotentStore.NHibernate
{
	using System;
	using System.Collections.Generic;
	using global::NHibernate;
	using global::NHibernate.Criterion;
	using IdempotentConsumer;

	public class NHibernateMessageStore : ILoadDispatchedMessages
	{
		private readonly ISession session;

		public NHibernateMessageStore(ISession session)
		{
			this.session = session;
		}
		public IEnumerable<DispatchedMessage> Load(Guid aggregateId, Guid messageId)
		{
			var criteria = this.session.CreateCriteria(typeof(DispatchedMessage));
			criteria.Add(Restrictions.Eq("AggregateId", aggregateId));
			criteria.Add(Restrictions.Eq("SourceMessageId", messageId));
			var messages = criteria.List();
			foreach (var message in messages)
				yield return message as DispatchedMessage;
		}
	}
}