namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	public class MessageBuilder : IBuildMessages,
		IRegisterMessageIdentifiers
	{
		private readonly DateTime now;
		private Guid currentMessageId;
		private Guid currentAggregateId;
		private int messageIndex;
		private int groupIndex;

		public MessageBuilder(DateTime now)
		{
			this.now = now;
		}

		public void RegisterMessageIdentifiers(Guid aggregateId, Guid messageId)
		{
			this.currentAggregateId = aggregateId;
			this.currentMessageId = messageId;
		}

		public IEnumerable<DispatchedMessage> Build(DispatchMethod method, params IMessage[] messages)
		{
			foreach (var message in messages)
				yield return this.Build(method, message);

			this.groupIndex++;
		}
		private DispatchedMessage Build(DispatchMethod method, IMessage message)
		{
			return new DispatchedMessage
			{
				SourceMessageId = this.currentMessageId,
				MessageIndex = this.messageIndex++,
				GroupIndex = this.groupIndex,
				Method = method,
				AggregateId = this.currentAggregateId,
				Body = message,
				Created = this.now
			};
		}
	}
}