namespace IdempotentConsumer
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	public class DuplicateMessageException : Exception
	{
		public DuplicateMessageException()
		{
		}
		public DuplicateMessageException(string message)
			: base(message)
		{
		}
		public DuplicateMessageException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		public DuplicateMessageException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public IEnumerable<DispatchedMessage> CommittedMessages { get; set; }
	}
}