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

namespace Ninject.Extensions.Interception.Attributes
{
    /// <summary>
    /// A baseline definition of an attribute that indicates one or more methods should be intercepted.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true )]
    public abstract class InterceptAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the interceptor's order number. Interceptors are invoked in ascending order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Creates the interceptor associated with the attribute.
        /// </summary>
        /// <param name="request">The request that is being intercepted.</param>
        /// <returns>The interceptor.</returns>
        public abstract IInterceptor CreateInterceptor( IProxyRequest request );
    }
}