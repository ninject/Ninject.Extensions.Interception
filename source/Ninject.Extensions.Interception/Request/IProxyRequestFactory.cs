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

using System;
using System.Reflection;
using Ninject.Activation;
using Ninject.Components;

#endregion

namespace Ninject.Extensions.Interception.Request
{
    /// <summary>
    /// Creates <see cref="IProxyRequest"/>s, which describe method calls.
    /// </summary>
    public interface IProxyRequestFactory : INinjectComponent
    {
        /// <summary>
        /// Creates a request representing the specified method call.
        /// </summary>
        /// <param name="context">The context in which the target instance was activated.</param>
        /// <param name="proxy">The proxy instance.</param>
        /// <param name="target">The target instance.</param>
        /// <param name="method">The method that will be called on the target instance.</param>
        /// <param name="arguments">The arguments to the method.</param>
        /// <param name="genericArguments">The generic type arguments for the method.</param>
        /// <returns>The newly-created request.</returns>
        IProxyRequest Create( IContext context,
                              object proxy,
                              object target,
                              MethodInfo method,
                              object[] arguments,
                              Type[] genericArguments );
    }
}