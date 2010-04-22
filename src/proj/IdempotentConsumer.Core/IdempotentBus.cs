namespace IdempotentConsumer.Core
{
	using System;
	using NServiceBus;

	public class IdempotentBus : ProxyBus, IIdempotentBus
	{
		private readonly ITrackUnitOfWork unitOfWork;

		public IdempotentBus(IBus bus, ITrackUnitOfWork unitOfWork)
			: base(bus)
		{
			this.unitOfWork = unitOfWork;
		}

		public override void Publish<T>(params T[] messages)
		{
			this.unitOfWork.RegisterNew(DispatchMethod.Publish, messages as IMessage[]);
		}
		public override void Publish<T>(Action<T> messageConstructor)
		{
			this.Publish(this.CreateInstance(messageConstructor));
		}

		public override void SendLocal(params IMessage[] messages)
		{
			this.unitOfWork.RegisterNew(DispatchMethod.SendLocal, messages);
		}
		public override void SendLocal<T>(Action<T> messageConstructor)
		{
			this.SendLocal(this.CreateInstance(messageConstructor));
		}

		public override ICallback Send(params IMessage[] messages)
		{
			this.unitOfWork.RegisterNew(DispatchMethod.Send, messages);
			return null; // we're not going down the callback route
		}
		public override ICallback Send<T>(Action<T> messageConstructor)
		{
			return this.Send(this.CreateInstance(messageConstructor));
		}

		public override void Reply(params IMessage[] messages)
		{
			this.unitOfWork.RegisterNew(DispatchMethod.Reply, messages);
		}
		public override void Reply<T>(Action<T> messageConstructor)
		{
			this.Reply(this.CreateInstance(messageConstructor));
		}

		public override void Return(int errorCode)
		{
			this.unitOfWork.RegisterNew(DispatchMethod.Return, new ReturnCodeMessage(errorCode));
		}
	}
}