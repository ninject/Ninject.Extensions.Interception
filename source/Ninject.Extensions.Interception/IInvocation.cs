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

using System.Collections.Generic;
using Ninject.Extensions.Interception.Request;

#endregion

namespace Ninject.Extensions.Interception
{
    /// <summary>
    /// Describes an executable invocation.
    /// </summary>
    public interface IInvocation
    {
        /// <summary>
        /// Gets the request, which describes the method call.
        /// </summary>
        IProxyRequest Request { get; }

        /// <summary>
        /// Gets the chain of interceptors that will be executed before the target method is called.
        /// </summary>
        IEnumerable<IInterceptor> Interceptors { get; }

        /// <summary>
        /// Gets or sets the return value for the method.
        /// </summary>
        object ReturnValue { get; set; }

        /// <summary>
        /// Continues the invocation, either by invoking the next interceptor in the chain, or
        /// if there are no more interceptors, calling the target method.
        /// </summary>
        void Proceed();
    }
}