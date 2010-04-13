namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;

	public class DispatchedMessageUnitOfWork : IDispatchedMessageUnitOfWork
	{
		private readonly HashSet<DispatchedMessage> tracked = new HashSet<DispatchedMessage>();
		private readonly IStoreDispatchedMessages messageStore;

		private DispatchMethod methodOfPreviousMessage;
		private int messageIndex;
		private int groupIndex;

		public DispatchedMessageUnitOfWork(IStoreDispatchedMessages messageStore)
		{
			this.messageStore = messageStore;
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
			this.messageStore.Store(this.tracked);
			this.tracked.Clear();
		}
	}
}