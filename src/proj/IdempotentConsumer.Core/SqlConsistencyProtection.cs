namespace IdempotentConsumer.Core
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Transactions;

	public class SqlConsistencyProtection : IProtectConsistency
	{
		private readonly ICollection<IDisposable> locks = new LinkedList<IDisposable>();
		private readonly IEnumerable<ConnectionStringSettings> available;
		private readonly int minimumLocks;

		public SqlConsistencyProtection(IEnumerable<ConnectionStringSettings> available, int minimumLocks)
		{
			this.available = available;
			this.minimumLocks = minimumLocks;
		}

		public void Lock(Guid id)
		{
			SuppressTransactionEnlistment(() => this.Acquire(id));
			if (this.locks.Count < this.minimumLocks)
				throw new ConsistencyProtectionException();
		}
		private static void SuppressTransactionEnlistment(Action action)
		{
			using (new TransactionScope(TransactionScopeOption.Suppress))
				action();
		}
		private void Acquire(Guid id)
		{
			foreach (var settings in this.available)
			{
				var connection = new SqlConsistencyLock(settings.ConnectionString, settings.ProviderName);
				if (connection.Acquire(id))
					this.locks.Add(connection);

				if (this.locks.Count >= this.minimumLocks)
					break;
			}
		}

		public void Dispose()
		{
			foreach (var handle in this.locks.Reverse())
				handle.Dispose();
		}
	}
}