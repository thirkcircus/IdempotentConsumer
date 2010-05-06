namespace IdempotentConsumer.Core
{
	using System;
	using System.Data;
	using System.Data.Common;

	public class SqlConsistencyLock : IDisposable
	{
		// TODO: expand dialects
		private const string SqlText = 
			"INSERT INTO ConsistencyHandles SELECT @p0; DELETE FROM ConsistencyHandles WHERE id = @p0;";
		private readonly IDbConnection connection;
		private IDbTransaction transaction;

		public SqlConsistencyLock(string connectionString, string providerName)
		{
			var factory = DbProviderFactories.GetFactory(providerName);
			this.connection = factory.CreateConnection();
			this.connection.ConnectionString = connectionString;
		}

		public bool Acquire(Guid id)
		{
			try
			{
				this.connection.Open(); // TODO: circuit breaker pattern?
				this.transaction = this.connection.BeginTransaction(IsolationLevel.ReadCommitted);
				this.EnterCriticalSection(id);
				return true;
			}
			catch
			{
				return false;
			}
		}
		private void EnterCriticalSection(Guid id)
		{
			using (var command = this.connection.CreateCommand())
			{
				command.CommandText = SqlText;
				var param = command.CreateParameter();
				param.ParameterName = "@p0";
				param.Value = id;
				command.Parameters.Add(param);
				command.ExecuteNonQuery(); // critial section; potential for timeouts
			}
		}

		public void Dispose()
		{
			try
			{
				if (this.transaction != null)
					this.transaction.Dispose(); // no need to commit

				this.connection.Dispose();
			}
			catch
			{
			}
		}
	}
}