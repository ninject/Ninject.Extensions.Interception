#region License

// 
// Author: Nate Kohari <nate@enkari.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
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
    /// Creates proxies for activated instances to allow method calls on them to be intercepted.
    /// </summary>
    public interface IProxyFactory : INinjectComponent
    {
        /// <summary>
        /// Wraps the instance in the specified context in a proxy.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <param name="reference"></param>
        /// <returns>A proxy that wraps the instance.</returns>
        void Wrap( IContext context, InstanceReference reference );

        /// <summary>
        /// Unwraps the instance in the specified context.
        /// </summary>
        /// <param name="context">The activation context.</param>
        /// <param name="reference"></param>
        /// <returns>The unwrapped instance.</returns>
        void Unwrap( IContext context, InstanceReference reference );
    }
}