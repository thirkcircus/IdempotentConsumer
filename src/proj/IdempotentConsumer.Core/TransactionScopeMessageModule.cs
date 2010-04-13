namespace IdempotentConsumer.Core
{
	using System.Transactions;
	using NServiceBus;

	public class TransactionScopeMessageModule : IMessageModule
	{
		private readonly TransactionScope scope;
		private readonly IDispatchedMessageUnitOfWork unitOfWork;

		public TransactionScopeMessageModule(TransactionScope scope, IDispatchedMessageUnitOfWork unitOfWork)
		{
			this.scope = scope;
			this.unitOfWork = unitOfWork;
		}

		public void HandleBeginMessage()
		{
		}

		public void HandleEndMessage()
		{
			this.unitOfWork.Complete();
			this.scope.Complete();
		}

		public void HandleError()
		{
		}
	}
}