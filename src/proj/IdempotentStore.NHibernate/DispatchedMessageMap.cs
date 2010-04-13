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
				.KeyProperty(x => x.SourceMessageId)
				.KeyProperty(x => x.MessageIndex);

			this.Map(x => x.GroupIndex);
			this.Map(x => x.Method);
			this.Map(x => x.AggregateId).Index("IX_DispatchedMessages_AggregateId");
			this.Map(x => x.Body).CustomType(typeof(byte[]));
			this.Map(x => x.Created);
		}
	}
}