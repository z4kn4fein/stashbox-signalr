using System;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Stashbox.Infrastructure;

namespace Stashbox.AspNet.SignalR
{
    /// <summary>
    /// Holds the <see cref="IStashboxContainer"/> extension methods for SignalR.
    /// </summary>
    public static class StashboxContainerExtensions
    {
        /// <summary>
        /// Adds <see cref="StashboxContainer"/> as the default dependency resolver and the default <see cref="IHubActivator"/>, also registers the available <see cref="IHub"/> and <see cref="PersistentConnection"/> implementations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer AddSignalR(this IStashboxContainer container, params Assembly[] assemblies)
        {
            container.RegisterInstance<Microsoft.AspNet.SignalR.IDependencyResolver>(new StashboxDependencyResolver(container));
            container.RegisterInstance<IHubActivator>(new StashboxHubActivator(container));
            GlobalHost.DependencyResolver = container.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>();

            return container.RegisterHubs(assemblies).RegisterPersistentConnections(assemblies);
        }

        /// <summary>
        /// Adds <see cref="StashboxContainer"/> as the default dependency resolver and the default <see cref="IHubActivator"/>, also registers the available <see cref="IHub"/> and <see cref="PersistentConnection"/> implementations.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="types">The types.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer AddSignalRWithTypes(this IStashboxContainer container, params Type[] types)
        {
            container.RegisterInstance<Microsoft.AspNet.SignalR.IDependencyResolver>(new StashboxDependencyResolver(container));
            container.RegisterInstance<IHubActivator>(new StashboxHubActivator(container));
            GlobalHost.DependencyResolver = container.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>();

            return container.RegisterHubs(types).RegisterPersistentConnections(types);
        }

        /// <summary>
        /// Registers the <see cref="IHub"/> implementations found in the given assemblies.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer RegisterHubs(this IStashboxContainer container, params Assembly[] assemblies)
        {
            if (assemblies.Length > 0)
                container.RegisterAssemblies(assemblies,
                    type => typeof(IHub).IsAssignableFrom(type),
                        context => context.WithoutDisposalTracking());

            return container;
        }

        /// <summary>
        /// Registers the <see cref="IHub"/> implementations found in the given types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="types">The types.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer RegisterHubs(this IStashboxContainer container, params Type[] types)
        {
            if (types.Length > 0)
                container.RegisterTypes(types, type => typeof(IHub).IsAssignableFrom(type), context => context.WithoutDisposalTracking());

            return container;
        }

        /// <summary>
        /// Registers the <see cref="PersistentConnection"/> implementations found in the given assemblies.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer RegisterPersistentConnections(this IStashboxContainer container, params Assembly[] assemblies)
        {
            if (assemblies.Length > 0)
                container.RegisterAssemblies(assemblies,
                    type => typeof(PersistentConnection).IsAssignableFrom(type),
                        context => context.WithoutDisposalTracking());

            return container;
        }

        /// <summary>
        /// Registers the <see cref="PersistentConnection"/> implementations found in the given types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="types">The types.</param>
        /// <returns>The container.</returns>
        public static IStashboxContainer RegisterPersistentConnections(this IStashboxContainer container, params Type[] types)
        {
            if (types.Length > 0)
                container.RegisterTypes(types, type => typeof(PersistentConnection).IsAssignableFrom(type), context => context.WithoutDisposalTracking());

            return container;
        }
    }
}
