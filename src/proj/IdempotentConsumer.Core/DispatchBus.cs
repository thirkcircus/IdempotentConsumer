namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	public class DispatchBus : ICaptureDispatchedMessages
	{
		private readonly IBus bus;
		private readonly IDispatchedMessageUnitOfWork unitOfWork;

		private Guid aggregateId;
		private Guid messageId;

		public DispatchBus(IBus bus, IDispatchedMessageUnitOfWork unitOfWork)
		{
			this.bus = bus;
			this.unitOfWork = unitOfWork;
		}

		public void AssignMessageIdentifiers(Guid currentAggregateId, Guid currentMessageId)
		{
			this.aggregateId = currentAggregateId;
			this.messageId = currentMessageId;
		}
		private IEnumerable<DispatchedMessage> Build(params IMessage[] messages)
		{
			foreach (var message in messages)
			{
				yield return new DispatchedMessage
				{
					SourceMessageId = this.messageId,
					AggregateId = this.aggregateId,
					Body = message,
					Created = DateTime.UtcNow // TODO
				};
			}
		}

		private void RegisterMessages(params IMessage[] messages)
		{
			foreach (var message in this.Build(messages))
				this.unitOfWork.RegisterNew(message);
		}

		public IDictionary<string, string> OutgoingHeaders
		{
			get { return this.bus.OutgoingHeaders; }
		}
		public IMessageContext CurrentMessageContext
		{
			get { return this.bus.CurrentMessageContext; }
		}

		public T CreateInstance<T>() where T : IMessage
		{
			return this.bus.CreateInstance<T>();
		}
		public T CreateInstance<T>(Action<T> action) where T : IMessage
		{
			return this.bus.CreateInstance(action);
		}
		public object CreateInstance(Type messageType)
		{
			return this.bus.CreateInstance(messageType);
		}

		public void Publish<T>(params T[] messages) where T : IMessage
		{
			this.RegisterMessages(messages as IMessage[]);
			this.bus.Publish(messages);
		}
		public void Publish<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Publish(this.CreateInstance(messageConstructor));
		}

		public void Subscribe(Type messageType)
		{
			this.bus.Subscribe(messageType);
		}
		public void Subscribe<T>() where T : IMessage
		{
			this.bus.Subscribe<T>();
		}
		public void Subscribe(Type messageType, Predicate<IMessage> condition)
		{
			this.bus.Subscribe(messageType, condition);
		}
		public void Subscribe<T>(Predicate<T> condition) where T : IMessage
		{
			this.bus.Subscribe(condition);
		}

		public void Unsubscribe(Type messageType)
		{
			this.bus.Unsubscribe(messageType);
		}
		public void Unsubscribe<T>() where T : IMessage
		{
			this.bus.Unsubscribe<T>();
		}

		public void SendLocal(params IMessage[] messages)
		{
			this.RegisterMessages(messages);
			this.bus.SendLocal(messages);
		}
		public void SendLocal<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.SendLocal(this.CreateInstance(messageConstructor));
		}

		public ICallback Send(params IMessage[] messages)
		{
			this.RegisterMessages(messages);
			return this.bus.Send(messages);
		}
		public ICallback Send<T>(Action<T> messageConstructor) where T : IMessage
		{
			return this.Send(this.CreateInstance(messageConstructor));
		}
		public ICallback Send(string destination, params IMessage[] messages)
		{
			return this.bus.Send(destination, messages);
		}
		public ICallback Send<T>(string destination, Action<T> messageConstructor) where T : IMessage
		{
			return this.bus.Send(destination, messageConstructor);
		}
		public void Send(string destination, string correlationId, params IMessage[] messages)
		{
			this.bus.Send(destination, correlationId, messages);
		}
		public void Send<T>(string destination, string correlationId, Action<T> messageConstructor) where T : IMessage
		{
			this.bus.Send(destination, correlationId, messageConstructor);
		}

		public void Reply(params IMessage[] messages)
		{
			this.RegisterMessages(messages);
			this.bus.Reply(messages);
		}
		public void Reply<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Reply(this.CreateInstance(messageConstructor));
		}

		public void Return(int errorCode)
		{
			this.RegisterMessages(new ReturnCodeMessage(errorCode));
			this.bus.Return(errorCode);
		}

		public void HandleCurrentMessageLater()
		{
			this.bus.HandleCurrentMessageLater();
		}
		public void ForwardCurrentMessageTo(string destination)
		{
			this.bus.ForwardCurrentMessageTo(destination);
		}
		public void DoNotContinueDispatchingCurrentMessageToHandlers()
		{
			this.bus.DoNotContinueDispatchingCurrentMessageToHandlers();
		}
	}
}