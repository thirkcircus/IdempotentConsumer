namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using NServiceBus;

	public abstract class BusProxy : IBus
	{
		protected BusProxy(IBus bus)
		{
			this.Bus = bus;
		}

		protected IBus Bus { get; private set; }

		public virtual IDictionary<string, string> OutgoingHeaders
		{
			get { return this.Bus.OutgoingHeaders; }
		}
		public virtual IMessageContext CurrentMessageContext
		{
			get { return this.Bus.CurrentMessageContext; }
		}

		public virtual T CreateInstance<T>() where T : IMessage
		{
			return this.Bus.CreateInstance<T>();
		}
		public virtual T CreateInstance<T>(Action<T> action) where T : IMessage
		{
			return this.Bus.CreateInstance(action);
		}
		public virtual object CreateInstance(Type messageType)
		{
			return this.Bus.CreateInstance(messageType);
		}

		public virtual void Publish<T>(params T[] messages) where T : IMessage
		{
			this.Bus.Publish(messages);
		}
		public virtual void Publish<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Publish(this.CreateInstance(messageConstructor));
		}

		public virtual void Subscribe(Type messageType)
		{
			this.Bus.Subscribe(messageType);
		}
		public virtual void Subscribe<T>() where T : IMessage
		{
			this.Bus.Subscribe<T>();
		}
		public virtual void Subscribe(Type messageType, Predicate<IMessage> condition)
		{
			this.Bus.Subscribe(messageType, condition);
		}
		public virtual void Subscribe<T>(Predicate<T> condition) where T : IMessage
		{
			this.Bus.Subscribe(condition);
		}

		public virtual void Unsubscribe(Type messageType)
		{
			this.Bus.Unsubscribe(messageType);
		}
		public virtual void Unsubscribe<T>() where T : IMessage
		{
			this.Bus.Unsubscribe<T>();
		}

		public virtual void SendLocal(params IMessage[] messages)
		{
			this.Bus.SendLocal(messages);
		}
		public virtual void SendLocal<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.SendLocal(this.CreateInstance(messageConstructor));
		}

		public virtual ICallback Send(params IMessage[] messages)
		{
			return this.Bus.Send(messages);
		}
		public virtual ICallback Send<T>(Action<T> messageConstructor) where T : IMessage
		{
			return this.Send(this.CreateInstance(messageConstructor));
		}
		public virtual ICallback Send(string destination, params IMessage[] messages)
		{
			return this.Bus.Send(destination, messages);
		}
		public virtual ICallback Send<T>(string destination, Action<T> messageConstructor) where T : IMessage
		{
			return this.Bus.Send(destination, messageConstructor);
		}
		public virtual void Send(string destination, string correlationId, params IMessage[] messages)
		{
			this.Bus.Send(destination, correlationId, messages);
		}
		public virtual void Send<T>(string destination, string correlationId, Action<T> messageConstructor) where T : IMessage
		{
			this.Bus.Send(destination, correlationId, messageConstructor);
		}

		public virtual void Reply(params IMessage[] messages)
		{
			this.Bus.Reply(messages);
		}
		public virtual void Reply<T>(Action<T> messageConstructor) where T : IMessage
		{
			this.Reply(this.CreateInstance(messageConstructor));
		}

		public virtual void Return(int errorCode)
		{
			this.Bus.Return(errorCode);
		}

		public virtual void HandleCurrentMessageLater()
		{
			this.Bus.HandleCurrentMessageLater();
		}
		public virtual void ForwardCurrentMessageTo(string destination)
		{
			this.Bus.ForwardCurrentMessageTo(destination);
		}
		public virtual void DoNotContinueDispatchingCurrentMessageToHandlers()
		{
			this.Bus.DoNotContinueDispatchingCurrentMessageToHandlers();
		}
	}
}