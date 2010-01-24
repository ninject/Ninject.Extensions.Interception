#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2010, Enkari, Ltd.
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using Ninject.Activation;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.ProxyFactory
{
    /// <summary>
    /// A baseline definition of a proxy factory.
    /// </summary>
    public abstract class ProxyFactoryBase : NinjectComponent, IProxyFactory
    {
        #region IProxyFactory Members

        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to wrap.</param>
        /// <returns>A proxy that wraps the instance.</returns>
        public abstract void Wrap( IContext context, InstanceReference reference );

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The context in which the instance was activated.</param>
        /// <param name="reference">The <see cref="InstanceReference"/> to unwrap.</param>
        /// <returns>The unwrapped instance.</returns>
        public abstract void Unwrap( IContext context, InstanceReference reference );

        #endregion
    }
}