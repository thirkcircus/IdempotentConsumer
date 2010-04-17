namespace IdempotentConsumer.Core
{
	using System;
	using System.Linq;

	public class DuplicateMessageFilter : IFilterDuplicateMessages
	{
		private readonly ILoadDispatchedMessages messageLoad;
		private readonly IDispatchMessages dispatcher;

		public DuplicateMessageFilter(ILoadDispatchedMessages repository, IDispatchMessages dispatcher)
		{
			this.messageLoad = repository;
			this.dispatcher = dispatcher;
		}

		public void Filter(Action handleMessage, Guid aggregateId, Guid messageId)
		{
			var previouslyDispatched = this.messageLoad.Load(aggregateId, messageId);
			if (previouslyDispatched.Any())
				this.dispatcher.Dispatch(previouslyDispatched);
			else
				handleMessage();
		}
	}
}