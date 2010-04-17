namespace IdempotentConsumer
{
	using System;

	public interface IRegisterMessageIdentifiers
	{
		void RegisterMessageIdentifiers(Guid aggregateId, Guid messageId);
	}
}