namespace IdempotentConsumer.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using NServiceBus;

	public class MessageDispatcher : IDispatchMessages
	{
		private readonly IBus bus;
		private readonly HashSet<DispatchedMessage> previouslyDispatched = new HashSet<DispatchedMessage>();

		public MessageDispatcher(IBus bus)
		{
			this.bus = bus;
		}

		public void Dispatch(IEnumerable<DispatchedMessage> messages)
		{
			foreach (var group in messages.GroupBy(x => x.GroupIndex))
				this.Dispatch(group.ToArray());
		}

		private void Dispatch(DispatchedMessage[] messages)
		{
			var method = messages[0].Method;
			var undispatched = this.GetUndispatchedMessages(messages).ToArray();
			this.bus.Dispatch(method, undispatched);
		}
		private IEnumerable<IMessage> GetUndispatchedMessages(IEnumerable<DispatchedMessage> messages)
		{
			return messages.Where(x => !this.MessageAlreadyDispatched(x)).Select(x => x.Payload);
		}

		private bool MessageAlreadyDispatched(DispatchedMessage message)
		{
			return !this.previouslyDispatched.Add(message);
		}
	}
}