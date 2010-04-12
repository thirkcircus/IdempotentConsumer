namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class DuplicateMessageFilter : IFilterDuplicateMessages
	{
		private readonly ILoadDispatchedMessages repository;
		private readonly IDispatchMessages dispatcher;

		public DuplicateMessageFilter(ILoadDispatchedMessages repository, IDispatchMessages dispatcher)
		{
			this.repository = repository;
			this.dispatcher = dispatcher;
		}

		public void Filter(Action handleMessage, Guid aggregateId, Guid messageId)
		{
			var messages = this.repository.Load(aggregateId, messageId);
			if (MessageHasNeverBeenHandled(messages))
				handleMessage();
			else
				this.dispatcher.Dispatch(messages);
		}

		private static bool MessageHasNeverBeenHandled(IEnumerable<DispatchedMessage> previouslyPublished)
		{
			return !previouslyPublished.Any();
		}
	}
}