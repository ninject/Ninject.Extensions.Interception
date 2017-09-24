// -------------------------------------------------------------------------------------------------
// <copyright file="ProxyFactoryBase.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.ProxyFactory
{
    using Ninject.Activation;
    using Ninject.Components;

    /// <summary>
    /// A baseline definition of a proxy factory.
    /// </summary>
    public abstract class ProxyFactoryBase : NinjectComponent, IProxyFactory
    {
        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to wrap.</param>
        public abstract void Wrap(IContext context, InstanceReference reference);

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to unwrap.</param>
        public abstract void Unwrap(IContext context, InstanceReference reference);
    }
}