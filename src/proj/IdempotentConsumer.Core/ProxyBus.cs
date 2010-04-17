namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	public abstract class ProxyBus : IBus
	{
		protected ProxyBus(IBus inner)
		{
			this.Inner = inner;
		}

		protected IBus Inner { get; private set; }

		public virtual IDictionary<string, string> OutgoingHeaders
		{
			get { return this.Inner.OutgoingHeaders; }
		}
		public virtual IMessageContext CurrentMessageContext
		{
			get { return this.Inner.CurrentMessageContext; }
		}

		public virtual T CreateInstance<T>() where T : IMessage
		{
			return this.Inner.CreateInstance<T>();
		}
		public virtual T CreateInstance<T>(Action<T> action) where T : IMessage
		{
			return this.Inner.CreateInstance(action);
		}
		public virtual object CreateInstance(Type messageType)
		{
			return this.Inner.CreateInstance(messageType);
		}

		public virtual void Publish<T>(params T[] messages) where T : IMessage
		{
			this.Inner.Publish(messages);
		}
		public virtual void Publish<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Publish(this.CreateInstance(messageConstructor));
		}

		public virtual void Subscribe(Type messageType)
		{
			this.Inner.Subscribe(messageType);
		}
		public virtual void Subscribe<T>() where T : IMessage
		{
			this.Inner.Subscribe<T>();
		}
		public virtual void Subscribe(Type messageType, Predicate<IMessage> condition)
		{
			this.Inner.Subscribe(messageType, condition);
		}
		public virtual void Subscribe<T>(Predicate<T> condition) where T : IMessage
		{
			this.Inner.Subscribe(condition);
		}

		public virtual void Unsubscribe(Type messageType)
		{
			this.Inner.Unsubscribe(messageType);
		}
		public virtual void Unsubscribe<T>() where T : IMessage
		{
			this.Inner.Unsubscribe<T>();
		}

		public virtual void SendLocal(params IMessage[] messages)
		{
			this.Inner.SendLocal(messages);
		}
		public virtual void SendLocal<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.SendLocal(this.CreateInstance(messageConstructor));
		}

		public virtual ICallback Send(params IMessage[] messages)
		{
			return this.Inner.Send(messages);
		}
		public virtual ICallback Send<T>(Action<T> messageConstructor) where T : IMessage
		{
			return this.Send(this.CreateInstance(messageConstructor));
		}
		public virtual ICallback Send(string destination, params IMessage[] messages)
		{
			return this.Inner.Send(destination, messages);
		}
		public virtual ICallback Send<T>(string destination, Action<T> messageConstructor) where T : IMessage
		{
			return this.Inner.Send(destination, messageConstructor);
		}
		public virtual void Send(string destination, string correlationId, params IMessage[] messages)
		{
			this.Inner.Send(destination, correlationId, messages);
		}
		public virtual void Send<T>(string destination, string correlationId, Action<T> messageConstructor) where T : IMessage
		{
			this.Inner.Send(destination, correlationId, messageConstructor);
		}

		public virtual void Reply(params IMessage[] messages)
		{
			this.Inner.Reply(messages);
		}
		public virtual void Reply<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Reply(this.CreateInstance(messageConstructor));
		}

		public virtual void Return(int errorCode)
		{
			this.Inner.Return(errorCode);
		}

		public virtual void HandleCurrentMessageLater()
		{
			this.Inner.HandleCurrentMessageLater();
		}
		public virtual void ForwardCurrentMessageTo(string destination)
		{
			this.Inner.ForwardCurrentMessageTo(destination);
		}
		public virtual void DoNotContinueDispatchingCurrentMessageToHandlers()
		{
			this.Inner.DoNotContinueDispatchingCurrentMessageToHandlers();
		}
	}
}