namespace IdempotentConsumer
{
	using System;
	using System.Runtime.Serialization;

	public class ConsistencyProtectionException : Exception
	{
		public ConsistencyProtectionException()
		{
		}
		public ConsistencyProtectionException(string message)
			: base(message)
		{
		}
		public ConsistencyProtectionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
		public ConsistencyProtectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}