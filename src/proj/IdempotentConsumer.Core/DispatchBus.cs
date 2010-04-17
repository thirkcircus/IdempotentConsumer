namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	public class DispatchBus : BusProxy, ICaptureDispatchedMessages
	{
		private readonly IDispatchedMessageUnitOfWork unitOfWork;
		private Guid aggregateId;
		private Guid messageId;

		public DispatchBus(IBus bus, IDispatchedMessageUnitOfWork unitOfWork)
			: base(bus)
		{
			this.unitOfWork = unitOfWork;
		}

		public void AssignMessageIdentifiers(Guid currentAggregateId, Guid currentMessageId)
		{
			this.aggregateId = currentAggregateId;
			this.messageId = currentMessageId;
		}
		private IEnumerable<DispatchedMessage> Build(DispatchMethod method, params IMessage[] messages)
		{
			foreach (var message in messages)
			{
				yield return new DispatchedMessage
				{
					SourceMessageId = this.messageId,
					AggregateId = this.aggregateId,
					Method = method,
					Body = message,
					Created = DateTime.UtcNow // TODO
				};
			}
		}

		private void RegisterMessages(DispatchMethod method, params IMessage[] messages)
		{
			foreach (var message in this.Build(method, messages))
				this.unitOfWork.RegisterNew(message);
		}

		public override void Publish<T>(params T[] messages)
		{
			this.RegisterMessages(DispatchMethod.Publish, messages as IMessage[]);
		}
		public override void Publish<T>(Action<T> messageConstructor)
		{
			this.Publish(this.CreateInstance(messageConstructor));
		}

		public override void SendLocal(params IMessage[] messages)
		{
			this.RegisterMessages(DispatchMethod.SendLocal, messages);
		}
		public override void SendLocal<T>(Action<T> messageConstructor)
		{
			this.SendLocal(this.CreateInstance(messageConstructor));
		}

		public override ICallback Send(params IMessage[] messages)
		{
			this.RegisterMessages(DispatchMethod.Send, messages);
			return null;
		}
		public override ICallback Send<T>(Action<T> messageConstructor)
		{
			return this.Send(this.CreateInstance(messageConstructor));
		}

		public override void Reply(params IMessage[] messages)
		{
			this.RegisterMessages(DispatchMethod.Reply, messages);
		}
		public override void Reply<T>(Action<T> messageConstructor)
		{
			this.Reply(this.CreateInstance(messageConstructor));
		}

		public override void Return(int errorCode)
		{
			this.RegisterMessages(DispatchMethod.Return, new ReturnCodeMessage(errorCode));
		}
	}
}