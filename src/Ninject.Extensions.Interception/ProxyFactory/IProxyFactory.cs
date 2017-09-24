// -------------------------------------------------------------------------------------------------
// <copyright file="IProxyFactory.cs" company="Ninject Project Contributors">
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
    /// Creates proxies for activated instances to allow method calls on them to be intercepted.
    /// </summary>
    public interface IProxyFactory : INinjectComponent
    {
        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <param name="reference">The instance reference.</param>
        void Wrap(IContext context, InstanceReference reference);

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <param name="reference">The instance reference.</param>
        void Unwrap(IContext context, InstanceReference reference);
    }
}