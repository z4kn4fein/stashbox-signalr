﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Stashbox.Infrastructure;

namespace Stashbox.AspNet.SignalR
{
    /// <summary>
    /// Represents a <see cref="DefaultDependencyResolver"/> using the <see cref="IStashboxContainer"/>.
    /// </summary>
    public class StashboxDependencyResolver : DefaultDependencyResolver
    {
        private readonly Infrastructure.IDependencyResolver dependencyResolver;

        /// <summary>
        /// Constructs a <see cref="StashboxDependencyResolver"/>.
        /// </summary>
        /// <param name="dependencyResolver">The container.</param>
        public StashboxDependencyResolver(Infrastructure.IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <inheritdoc />
        public override object GetService(Type serviceType) =>
            this.dependencyResolver.Resolve(serviceType, nullResultAllowed: true) ?? base.GetService(serviceType);

        /// <inheritdoc />
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var services = this.dependencyResolver.ResolveAll(serviceType);
            return services.Any() ? services : base.GetServices(serviceType);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing) => this.dependencyResolver.Dispose();
    }
}