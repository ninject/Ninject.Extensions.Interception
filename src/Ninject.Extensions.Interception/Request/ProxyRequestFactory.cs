// -------------------------------------------------------------------------------------------------
// <copyright file="ProxyRequestFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Extensions.Interception.Request
{
    using System;
    using System.Reflection;
    using Ninject.Activation;
    using Ninject.Components;

    /// <summary>
    /// The stock definition of a <see cref="IProxyRequestFactory"/>.
    /// </summary>
    public class ProxyRequestFactory : NinjectComponent, IProxyRequestFactory
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
        public IProxyRequest Create(
            IContext context,
            object proxy,
            object target,
            MethodInfo method,
            object[] arguments,
            Type[] genericArguments)
        {
            if (method.IsGenericMethodDefinition)
            {
                method = method.MakeGenericMethod(genericArguments);
            }

            return new ProxyRequest(context, proxy, target, method, arguments, genericArguments);
        }
    }
}