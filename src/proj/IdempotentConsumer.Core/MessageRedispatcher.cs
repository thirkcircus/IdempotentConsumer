namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using NServiceBus;

	public class MessageRedispatcher : IRedispatchMessages
	{
		private readonly IBus bus;
		private readonly HashSet<DispatchedMessage> alreadyDispatched = new HashSet<DispatchedMessage>();

		public MessageRedispatcher(IBus bus)
		{
			this.bus = bus;
		}

		public void Redispatch(IEnumerable<DispatchedMessage> messages)
		{
			foreach (var group in messages.GroupBy(x => x.GroupIndex))
			{
				this.bus.Dispatch(
					group.First().Method,
					this.GetUndispatchedMessages(group).ToArray());
			}
		}
		private IEnumerable<IMessage> GetUndispatchedMessages(IEnumerable<DispatchedMessage> messages)
		{
			return messages.Where(x => !this.MessageAlreadyDispatched(x)).Select(x => x.Body);
		}
		private bool MessageAlreadyDispatched(DispatchedMessage message)
		{
			return !this.alreadyDispatched.Add(message);
		}
	}
}