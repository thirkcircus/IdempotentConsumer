namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NServiceBus;

	internal static class BusExtensions
	{
		public static void Dispatch(this IBus bus, DispatchMethod method, IEnumerable<IMessage> messages)
		{
			var messagesToDispatch = messages.ToArray();
			if (messagesToDispatch.Length > 0)
				GetDispatcher(bus, method).Invoke(messagesToDispatch);
		}

		private static Action<IMessage[]> GetDispatcher(IBus bus, DispatchMethod method)
		{
			switch (method)
			{
				case DispatchMethod.Publish:
					return bus.Publish;
				case DispatchMethod.Reply:
					return bus.Reply;
				case DispatchMethod.Return:
					return bus.Return;
				case DispatchMethod.Send:
					return messages => bus.Send(messages);
				case DispatchMethod.SendLocal:
					return bus.SendLocal;
				default:
					throw new NotSupportedException();
			}
		}

		private static void Return(this IBus bus, IEnumerable<IMessage> messages)
		{
			foreach (var message in messages)
				bus.Return((message as ReturnCodeMessage));
		}
	}
}