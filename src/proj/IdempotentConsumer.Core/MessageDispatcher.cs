namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using NServiceBus;

	public class MessageDispatcher : IDispatchMessages
	{
		private readonly IBus bus;

		public MessageDispatcher(IBus bus)
		{
			this.bus = bus;
		}

		public void Dispatch(IEnumerable<DispatchedMessage> messages)
		{
			foreach (var group in messages.GroupBy(x => x.GroupIndex))
			{
				var method = group.First().Method;
				this.bus.Dispatch(method, group.Select(x => x.Body).ToArray());
			}
		}
	}
}