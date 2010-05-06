namespace IdempotentConsumer
{
	using System;

	public interface IProtectConsistency : IDisposable
	{
		void Lock(Guid id);
	}
}