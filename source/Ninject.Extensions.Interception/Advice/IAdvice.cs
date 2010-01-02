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
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception.Advice
{
    /// <summary>
    /// A declaration of advice, which relates a method or condition with an interceptor.
    /// </summary>
    public interface IAdvice
    {
        /// <summary>
        /// Gets or sets the method handle for the advice, if it is static.
        /// </summary>
        RuntimeMethodHandle MethodHandle { get; set; }

        /// <summary>
        /// Gets or sets the condition for the advice, if it is dynamic.
        /// </summary>
        Predicate<IProxyRequest> Condition { get; set; }

        /// <summary>
        /// Gets or sets the interceptor associated with the advice, if one was defined during
        /// registration.
        /// </summary>
        IInterceptor Interceptor { get; set; }

        /// <summary>
        /// Gets or sets the callback that will be triggered to create the interceptor, if applicable.
        /// </summary>
        Func<IProxyRequest, IInterceptor> Callback { get; set; }

        /// <summary>
        /// Gets the order of precedence in which the advice should be called.
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Gets a value indicating whether the advice is related to a condition instead of a
        /// specific method.
        /// </summary>
        bool IsDynamic { get; }

        /// <summary>
        /// Determines whether the advice matches the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns><see langword="True"/> if the request matches, otherwise <see langword="false"/>.</returns>
        bool Matches( IProxyRequest request );

        /// <summary>
        /// Gets the interceptor associated with the advice for the specified request.
        /// </summary>
        /// <param name="request">The request in question.</param>
        /// <returns>The interceptor.</returns>
        IInterceptor GetInterceptor( IProxyRequest request );
    }
}