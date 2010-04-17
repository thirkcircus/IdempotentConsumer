namespace IdempotentConsumer.Core
{
	using NServiceBus;

	internal class ReturnCodeMessage : IMessage
	{
		private readonly int returnCode;

		public ReturnCodeMessage(int returnCode)
		{
			this.returnCode = returnCode;
		}

		public static implicit operator int(ReturnCodeMessage message)
		{
			return message.returnCode;
		}
		public static implicit operator ReturnCodeMessage(int value)
		{
			return new ReturnCodeMessage(value);
		}
	}
}