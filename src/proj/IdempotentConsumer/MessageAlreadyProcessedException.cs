namespace IdempotentConsumer
{
	using System;
	using System.Runtime.Serialization;

	public class MessageAlreadyProcessedException : Exception
	{
		public MessageAlreadyProcessedException()
		{
		}
		public MessageAlreadyProcessedException(string message)
			: base(message)
		{
		}
		public MessageAlreadyProcessedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		public MessageAlreadyProcessedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}