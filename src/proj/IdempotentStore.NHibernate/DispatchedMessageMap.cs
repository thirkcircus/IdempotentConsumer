namespace IdempotentStore.NHibernate
{
	using FluentNHibernate.Mapping;
	using IdempotentConsumer;

	public sealed class DispatchedMessageMap : ClassMap<DispatchedMessage>
	{
		public DispatchedMessageMap()
		{
			this.Table("DispatchedMessages");

			this.CompositeId()
				.KeyProperty(x => x.AggregateId)
				.KeyProperty(x => x.GroupIndex)
				.KeyProperty(x => x.MessageIndex);

			this.Map(x => x.SourceMessageId).Index("IX_SourceMessageId");
			this.Map(x => x.Method);
			this.Map(x => x.Body).CustomType(typeof(byte[]));
			this.Map(x => x.Created);
		}
	}
}