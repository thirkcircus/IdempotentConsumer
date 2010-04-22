namespace IdempotentConsumer.Autofac
{
	using System;
	using Core;
	using global::Autofac.Builder;
	using NServiceBus;

	public class IdempotentConfigurationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// TODO: any message handler that implements IHandleMessagesOnce<T>
			// should have IdempotentBus injected.

			builder
				.Register(c => new DuplicateMessageFilter(
					c.Resolve<ILoadMessages>(),
					c.Resolve<IDispatchMessages>()))
				.As<IFilterDuplicateMessages>()
				.ContainerScoped()
				.ExternallyOwned();

			builder
				.Register(c => new IdempotentUnitOfWork(
					c.Resolve<IStoreMessages>(),
					c.Resolve<IDispatchMessages>(),
					c.Resolve<IBuildMessages>()))
				.As<ITrackUnitOfWork>()
				.ContainerScoped()
				.ExternallyOwned();

			builder
				.Register(c => new IdempotentBus(
					c.Resolve<IBus>(),
					c.Resolve<ITrackUnitOfWork>()))
				.As<IIdempotentBus>()
				.ContainerScoped()
				.ExternallyOwned();

			builder
				.Register(c => new MessageDispatcher(c.Resolve<IBus>()))
				.As<IDispatchMessages>()
				.ContainerScoped()
				.ExternallyOwned();

			builder
				.Register(c => new MessageBuilder(DateTime.UtcNow))
				.As<IBuildMessages>()
				.As<IRegisterMessageIdentifiers>()
				.ContainerScoped()
				.ExternallyOwned();

			builder
				.Register(c => new InMemoryStorage())
				.As<IStoreMessages>()
				.As<ILoadMessages>()
				.SingletonScoped()
				.ExternallyOwned();
		}
	}
}