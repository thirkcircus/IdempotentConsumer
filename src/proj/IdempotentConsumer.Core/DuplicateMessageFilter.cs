namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class DuplicateMessageFilter : IFilterDuplicateMessages
	{
		private readonly IStoreDispatchedMessages messageStore;
		private readonly IDispatchMessages dispatcher;

		public DuplicateMessageFilter(IStoreDispatchedMessages repository, IDispatchMessages dispatcher)
		{
			this.messageStore = repository;
			this.dispatcher = dispatcher;
		}

		public void Filter(Action handleMessage, Guid aggregateId, Guid messageId)
		{
			var messages = this.messageStore.Load(aggregateId, messageId);
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