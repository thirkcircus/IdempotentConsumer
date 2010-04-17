namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;

	public class DispatchedMessageUnitOfWork : IDispatchedMessageUnitOfWork
	{
		private readonly HashSet<DispatchedMessage> tracked = new HashSet<DispatchedMessage>();
		private readonly IStoreDispatchedMessages messageStore;
		private readonly IDispatchMessages dispatcher;

		private DispatchMethod methodOfPreviousMessage;
		private int messageIndex;
		private int groupIndex;

		public DispatchedMessageUnitOfWork(IStoreDispatchedMessages messageStore, IDispatchMessages dispatcher)
		{
			this.messageStore = messageStore;
			this.dispatcher = dispatcher;
		}

		public void RegisterNew(DispatchedMessage message)
		{
			if (!this.AddUntrackedMessage(message))
				return;

			this.IncrementGroupIndex(message.Method);
			message.GroupIndex = this.groupIndex;
			message.MessageIndex = this.messageIndex++;
		}
		private bool AddUntrackedMessage(DispatchedMessage message)
		{
			return this.tracked.Add(message);
		}
		private void IncrementGroupIndex(DispatchMethod messageMethod)
		{
			if (this.methodOfPreviousMessage != messageMethod)
				this.groupIndex++;

			this.methodOfPreviousMessage = messageMethod;
		}

		public void Complete()
		{
			try
			{
				this.messageStore.Store(this.tracked);
			}
			catch (MessageAlreadyProcessedException)
			{
				this.tracked.Clear(); // TODO: grab previously dispatched messages from exception and dispatch
			}

			this.dispatcher.Dispatch(this.tracked);
			this.tracked.Clear();
		}
	}
}